using AutoMapper;
using BenchmarkDotNet.Attributes;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Dotnet.Tests
{
    /// <summary>
    /// 对象映射性能测试
    /// |         Method |           Job |       Runtime |     Mean |     Error |    StdDev |   Gen 0 |   Gen 1 | Gen 2 | Allocated |
    ///| AutoMapperTest |    .NET 4.7.2 |    .NET 4.7.2 | 7.810 ms | 0.6416 ms | 1.8716 ms |       - |       - |     - |    864 KB |
    ///|    MapsterTest |    .NET 4.7.2 |    .NET 4.7.2 | 3.930 ms | 0.4057 ms | 1.1898 ms |       - |       - |     - |    280 KB |
    ///| AutoMapperTest | .NET Core 3.1 | .NET Core 3.1 | 3.686 ms | 0.0733 ms | 0.0650 ms | 85.9375 | 23.4375 |     - | 564.77 KB |
    ///|    MapsterTest | .NET Core 3.1 | .NET Core 3.1 | 1.436 ms | 0.0268 ms | 0.0263 ms | 31.2500 |  7.8125 |     - | 196.23 KB |
    ///| AutoMapperTest | .NET Core 5.0 | .NET Core 5.0 | 3.315 ms | 0.0662 ms | 0.0814 ms | 85.9375 | 27.3438 |     - | 541.81 KB |
    ///|    MapsterTest | .NET Core 5.0 | .NET Core 5.0 | 1.314 ms | 0.0256 ms | 0.0405 ms | 31.2500 |  7.8125 |     - | 196.35 KB |
    /// </summary>
    /// 设置导出文件格式
    [AsciiDocExporter, CsvExporter, RPlotExporter]
    ///设置同时运行的框架 <TargetFrameworks>net5.0;net472;netcoreapp3.1</TargetFrameworks> 与 SimpleJob一起使用
    [SimpleJob(BenchmarkDotNet.Jobs.RuntimeMoniker.NetCoreApp31)]
    [SimpleJob(BenchmarkDotNet.Jobs.RuntimeMoniker.NetCoreApp50)]
    [SimpleJob(BenchmarkDotNet.Jobs.RuntimeMoniker.Net472)]
    ///加上内存分析
    [MemoryDiagnoser]
    public class ObjectMapperTest
    {
        private readonly List<Student> m_students;

        public ObjectMapperTest()
        {
            Random random = new Random();

            m_students = new List<Student>();
            for (int i = 0; i < 20; i++)
            {
                m_students.Add(new Student()
                {
                    Age = random.Next(1, 100),
                    CID = DateTime.Now.ToString(),
                    Id = Guid.NewGuid().ToString(),
                    Name = i.ToString(),
                    Cource = new Cource()
                    {
                        ID = $"{i}{i}",
                        CourceName = "Hello Word",
                        Grade = i * 5
                    }
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Benchmark]
        public List<StudentDto> AutoMapperTest()
        {
            //.net core  使用 AutoMapper
            //1.注入
            //services.AddAutoMapper(assembly);
            // 2.配置
            //public class AutoMapperConfigs : Profile { 
            //       CreateMap<FileRecordModel, FileRecordResponse>()
            //        .ForMember(o => o.Id, t => t.MapFrom(d => d.Id.ToString()))
            //        .ReverseMap();
            //}
            //3.实现
            //public class WrapperOpenComponents
            //{
            //    private IMapper m_mapper { get; }
            //    public WrapperOpenComponents(IMapper mapper)
            //    {
            //        m_mapper = mapper;
            //    }
            //}

            var configuration = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Student, StudentDto>()
                    .ForMember(o => o.CourceName, t => t.MapFrom(d => d.Cource.CourceName))
                     .ForMember(o => o.Grade, t => t.MapFrom(d => d.Cource.Grade));
                });

            var mapper = configuration.CreateMapper();

            return mapper.Map<List<StudentDto>>(m_students);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Benchmark]
        public List<StudentDto> MapsterTest()
        {
            //.net core  使用 Mapster 
            //1.注入
            //TypeAdapterConfig.GlobalSettings.Scan(assembly);
            //2.配置
            // public class MappingRegister : IRegister
            //{
            //    public void Register(TypeAdapterConfig config)
            //    {
            //        config.ForType<Student, StudentDto>()
            //         .Map(dto => dto.CourceName, s => s.Cource.CourceName)
            //         .Map(dto => dto.Grade, s => s.Cource.Grade);
            //    }
            //}
            //3.实现
            //students.Adapt<List<StudentDto>>();

            List<StudentDto> dataList = null;

            var config = new TypeAdapterConfig();
            config.NewConfig<Student, StudentDto>()
            .Map(dto => dto.Id, d => d.Id)
            .Map(dto => dto.Name, d => d.Name)
            .Map(dto => dto.Age, d => d.Age)
            .Map(dto => dto.CourceName, s => s.Cource.CourceName)
            .Map(dto => dto.Grade, s => s.Cource.Grade);

            dataList = m_students.Adapt<List<StudentDto>>(config);

            return dataList;
        }

    }

    public class Student
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string CID { get; set; }
        public Cource Cource { get; set; }
    }

    public class Cource
    {
        public string ID { get; set; }
        public string CourceName { get; set; }
        public double Grade { get; set; }
    }

    public class StudentDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string CourceName { get; set; }
        public double Grade { get; set; }

    }

}
