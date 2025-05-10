// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using AutoMapper;
// using Pessoas.Core.Domain.Enums;
// using Pessoas.Core.Domain.Interfaces.Repository;
// using Pessoas.Core.Domain.Models;
// using Pessoas.Core.Domain.ViewModels;
// using Moq;
// using Xunit;
// using Pessoas.Core.Service.Implementations;
// using System.Linq.Expressions;
// using Pessoas.Core.Domain.Interfaces.APIs;
// using Pessoas.Core.Domain.ViewModels.Rota;

// namespace Pessoas.Core.Tests.Unitary;

// public class AlunoServiceTest
// {
//     private readonly Mock<IRouterAPI> _routerAPI;
//     private readonly Mock<IBaseRepository<Domain.Models.Aluno>> _alunoRepositoryMock;
//     // private readonly Mock<IBaseRepository<AlunoRota>> _alunoRotaRepositoryMock;
//     private readonly Mock<IUserContext> _userContextMock;
//     private readonly IMapper _mapper;
//     private readonly AlunoService _alunoService;

//     public AlunoServiceTest()
//     {
//         _routerAPI = new Mock<IRouterAPI>();
//         // _alunoRotaRepositoryMock = new Mock<IBaseRepository<AlunoRota>>();
//         _alunoRepositoryMock = new Mock<IBaseRepository<Domain.Models.Aluno>>();
//         _userContextMock = new Mock<IUserContext>();

//         var mapperConfig = new MapperConfiguration(cfg =>
//         {
//             cfg.CreateMap<Domain.Models.Aluno, AlunoViewModel>();
//         });
//         _mapper = mapperConfig.CreateMapper();

//         _alunoService = new AlunoService(
//             _routerAPI.Object,
//             _alunoRepositoryMock.Object,
//             _alunoRotaRepositoryMock.Object,
//             _userContextMock.Object,
//             _mapper
//         );
//     }

//     [Fact]
//     public async Task ObterAlunosPorFiltro_NoMatchingStudents_ReturnsEmptyList()
//     {
//         // Arrange
//         int rotaId = 1;
//         string filtro = "nonexistent";
//         _userContextMock.Setup(x => x.Empresa).Returns(1);

//         _routerAPI.Setup(x => x.ObterRotaPorIdAsync(It.IsAny<int>()))
//             .ReturnsAsync(new RotaViewModel());

//         // Act
//         var result = await _alunoService.ObterAlunosPorFiltro(rotaId, filtro);

//         // Assert
//         Assert.NotNull(result);
//         Assert.Empty(result);
//     }

//     [Fact]
//     public async Task ObterAlunosPorFiltro_StudentsInRoute_AreExcludedFromResult()
//     {
//         // Arrange
//         int rotaId = 1;
//         string filtro = "test";
//         _userContextMock.Setup(x => x.Empresa).Returns(1);

//         var alunos = new List<Domain.Models.Aluno>
//         {
//             new Domain.Models.Aluno { Id = 1, PrimeiroNome = "Test", UltimoNome = "Student", Status = StatusEntityEnum.Ativo, EmpresaId = 1 },
//             new Domain.Models.Aluno { Id = 2, PrimeiroNome = "Another", UltimoNome = "Student", Status = StatusEntityEnum.Ativo, EmpresaId = 1 }
//         };

//         var alunosNaRota = new List<AlunoRota>
//         {
//             new AlunoRota { AlunoId = 1, RotaId = rotaId, Status = StatusEntityEnum.Ativo }
//         };

//         _alunoRepositoryMock.Setup(x => x.BuscarAsync(
//             It.IsAny<Expression<Func<Domain.Models.Aluno, bool>>>(),
//             It.IsAny<Expression<Func<Domain.Models.Aluno, object>>[]>()))
//             .ReturnsAsync(alunos);

//         _alunoRotaRepositoryMock.Setup(x => x.BuscarAsync(It.IsAny<Expression<Func<AlunoRota, bool>>>()))
//             .ReturnsAsync(alunosNaRota);

//         // Act
//         var result = await _alunoService.ObterAlunosPorFiltro(rotaId, filtro);

//         // Assert
//         Assert.NotNull(result);
//         Assert.Single(result);
//         Assert.Equal(2, result.First().Id); // Only the student not in the route should be returned
//     }

//     [Fact]
//     public async Task ObterAlunosPorFiltro_StudentsNotInRoute_AreIncludedInResult()
//     {
//         // Arrange
//         int rotaId = 1;
//         string filtro = "test";
//         _userContextMock.Setup(x => x.Empresa).Returns(1);

//         var alunos = new List<Domain.Models.Aluno>
//         {
//             new Domain.Models.Aluno { Id = 1, PrimeiroNome = "Test", UltimoNome = "Student", Status = StatusEntityEnum.Ativo, EmpresaId = 1 },
//             new Domain.Models.Aluno { Id = 2, PrimeiroNome = "Another", UltimoNome = "Student", Status = StatusEntityEnum.Ativo, EmpresaId = 1 }
//         };

//         var alunosNaRota = new List<AlunoRota>(); // No students in the route

//         _alunoRepositoryMock.Setup(x => x.BuscarAsync(
//             It.IsAny<Expression<Func<Domain.Models.Aluno, bool>>>(),
//             It.IsAny<Expression<Func<Domain.Models.Aluno, object>>[]>()))
//             .ReturnsAsync(alunos);

//         _alunoRotaRepositoryMock.Setup(x => x.BuscarAsync(It.IsAny<Expression<Func<AlunoRota, bool>>>()))
//             .ReturnsAsync(alunosNaRota);

//         // Act
//         var result = await _alunoService.ObterAlunosPorFiltro(rotaId, filtro);

//         // Assert
//         Assert.NotNull(result);
//         Assert.Equal(2, result.Count); // Both students should be returned
//     }

//     [Fact]
//     public async Task ObterAlunosPorFiltro_EmptyFilter_ReturnsAllMatchingStudents()
//     {
//         // Arrange
//         int rotaId = 1;
//         string filtro = "";
//         _userContextMock.Setup(x => x.Empresa).Returns(1);

//         var alunos = new List<Domain.Models.Aluno>
//         {
//             new Domain.Models.Aluno { Id = 1, PrimeiroNome = "Test", UltimoNome = "Student", Status = StatusEntityEnum.Ativo, EmpresaId = 1 },
//             new Domain.Models.Aluno { Id = 2, PrimeiroNome = "Another", UltimoNome = "Student", Status = StatusEntityEnum.Ativo, EmpresaId = 1 }
//         };

//         _alunoRepositoryMock.Setup(x => x.BuscarAsync(
//             It.IsAny<Expression<Func<Domain.Models.Aluno, bool>>>(),
//             It.IsAny<Expression<Func<Domain.Models.Aluno, object>>[]>()))
//             .ReturnsAsync(alunos);

//         _alunoRotaRepositoryMock.Setup(x => x.BuscarAsync(It.IsAny<Expression<Func<AlunoRota, bool>>>()))
//             .ReturnsAsync(new List<AlunoRota>());

//         // Act
//         var result = await _alunoService.ObterAlunosPorFiltro(rotaId, filtro);

//         // Assert
//         Assert.NotNull(result);
//         Assert.Equal(2, result.Count);
//     }
// }