using System.Reflection;

using HotelReservaBack.Api.Controllers;
using HotelReservaBack.Application.DTOs;
using HotelReservaBack.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;

using Shouldly;

namespace HotelReservaBack.Tests.Controllers;

public class CrudControllersTests
{
    public static TheoryData<Type, Type> Controllers =>
        new TheoryData<Type, Type>
        {
            { typeof(PaisesController), typeof(PaisDto) },
            { typeof(RegionesController), typeof(RegionDto) },
            { typeof(CiudadesController), typeof(CiudadDto) },
            { typeof(ComunasController), typeof(ComunaDto) },
            { typeof(PerfilesController), typeof(PerfilDto) },
            { typeof(UsuariosController), typeof(UsuarioDto) },
            { typeof(EstadosReservaController), typeof(EstadoReservaDto) },
            { typeof(EstadosPagoController), typeof(EstadoPagoDto) },
            { typeof(HotelesController), typeof(HotelDto) },
            { typeof(TiposHabitacionController), typeof(TipoHabitacionDto) },
            { typeof(HabitacionesController), typeof(HabitacionDto) },
            { typeof(ClientesController), typeof(ClienteDto) },
            { typeof(ReservasController), typeof(ReservaDto) },
            { typeof(ReservasDetallesController), typeof(ReservaDetalleDto) },
            { typeof(PagosController), typeof(PagoDto) }
        };

    [Theory, MemberData(nameof(Controllers))]
    public async Task ObtenerTodos_RetornaOkParaCadaControlador(Type controllerType, Type dtoType)
    {
        ControllerContext context = CreateContext(controllerType, dtoType);
        context.Stub.Item = context.Dto;

        IActionResult result = await InvokeAsync(context.Controller, "ObtenerTodos", CancellationToken.None);

        OkObjectResult ok = result.ShouldBeOfType<OkObjectResult>();
        ok.Value.ShouldNotBeNull();
    }

    [Theory, MemberData(nameof(Controllers))]
    public async Task ObtenerPorId_CuandoExiste_RetornaOk(Type controllerType, Type dtoType)
    {
        ControllerContext context = CreateContext(controllerType, dtoType);
        context.Stub.Item = context.Dto;

        IActionResult result = await InvokeAsync(
            context.Controller,
            "ObtenerPorId",
            1,
            CancellationToken.None);

        OkObjectResult ok = result.ShouldBeOfType<OkObjectResult>();
        ok.Value.ShouldBeSameAs(context.Dto);
    }

    [Theory, MemberData(nameof(Controllers))]
    public async Task ObtenerPorId_CuandoNoExiste_RetornaNotFound(Type controllerType, Type dtoType)
    {
        ControllerContext context = CreateContext(controllerType, dtoType);

        IActionResult result = await InvokeAsync(
            context.Controller,
            "ObtenerPorId",
            999,
            CancellationToken.None);

        result.ShouldBeOfType<NotFoundResult>();
    }

    [Theory, MemberData(nameof(Controllers))]
    public async Task Crear_RetornaCreated(Type controllerType, Type dtoType)
    {
        ControllerContext context = CreateContext(controllerType, dtoType);
        context.Stub.Item = context.Dto;

        IActionResult result = await InvokeAsync(
            context.Controller,
            "Crear",
            context.Dto,
            CancellationToken.None);

        CreatedResult created = result.ShouldBeOfType<CreatedResult>();
        created.Value.ShouldBeSameAs(context.Dto);
    }

    [Theory, MemberData(nameof(Controllers))]
    public async Task Actualizar_CuandoExiste_RetornaNoContent(Type controllerType, Type dtoType)
    {
        ControllerContext context = CreateContext(controllerType, dtoType);
        context.Stub.OperationResult = true;

        IActionResult result = await InvokeAsync(
            context.Controller,
            "Actualizar",
            1,
            context.Dto,
            CancellationToken.None);

        result.ShouldBeOfType<NoContentResult>();
    }

    [Theory, MemberData(nameof(Controllers))]
    public async Task Actualizar_CuandoNoExiste_RetornaNotFound(Type controllerType, Type dtoType)
    {
        ControllerContext context = CreateContext(controllerType, dtoType);

        IActionResult result = await InvokeAsync(
            context.Controller,
            "Actualizar",
            999,
            context.Dto,
            CancellationToken.None);

        result.ShouldBeOfType<NotFoundResult>();
    }

    [Theory, MemberData(nameof(Controllers))]
    public async Task Eliminar_CuandoExiste_RetornaNoContent(Type controllerType, Type dtoType)
    {
        ControllerContext context = CreateContext(controllerType, dtoType);
        context.Stub.OperationResult = true;

        IActionResult result = await InvokeAsync(
            context.Controller,
            "Eliminar",
            1,
            CancellationToken.None);

        result.ShouldBeOfType<NoContentResult>();
    }

    [Theory, MemberData(nameof(Controllers))]
    public async Task Eliminar_CuandoNoExiste_RetornaNotFound(Type controllerType, Type dtoType)
    {
        ControllerContext context = CreateContext(controllerType, dtoType);

        IActionResult result = await InvokeAsync(
            context.Controller,
            "Eliminar",
            999,
            CancellationToken.None);

        result.ShouldBeOfType<NotFoundResult>();
    }

    private static ControllerContext CreateContext(Type controllerType, Type dtoType)
    {
        object dto = Activator.CreateInstance(dtoType)
            ?? throw new InvalidOperationException($"No fue posible crear {dtoType.Name}.");
        Type stubType = typeof(StubCrudService<>).MakeGenericType(dtoType);
        IStubCrudService stub = (IStubCrudService)(Activator.CreateInstance(stubType)
            ?? throw new InvalidOperationException($"No fue posible crear el stub para {dtoType.Name}."));
        object controller = Activator.CreateInstance(controllerType, stub)
            ?? throw new InvalidOperationException($"No fue posible crear {controllerType.Name}.");

        return new ControllerContext(controller, dto, stub);
    }

    private static async Task<IActionResult> InvokeAsync(
        object controller,
        string methodName,
        params object[] arguments)
    {
        MethodInfo method = controller.GetType().GetMethod(methodName)
            ?? throw new InvalidOperationException($"No existe el método {methodName}.");
        Task<IActionResult> task = (Task<IActionResult>)(method.Invoke(controller, arguments)
            ?? throw new InvalidOperationException($"El método {methodName} no retornó una tarea."));

        return await task;
    }

    private sealed record ControllerContext(object Controller, object Dto, IStubCrudService Stub);

    private interface IStubCrudService
    {
        object? Item { get; set; }
        bool OperationResult { get; set; }
    }

    private sealed class StubCrudService<TDto> : ICrudService<TDto>, IStubCrudService
        where TDto : class
    {
        public object? Item { get; set; }
        public bool OperationResult { get; set; }

        public Task<IReadOnlyList<TDto>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
        {
            IReadOnlyList<TDto> items = Item is TDto dto ? [dto] : [];
            return Task.FromResult(items);
        }

        public Task<TDto?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Item as TDto);
        }

        public Task<TDto> CrearAsync(TDto dto, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Item as TDto ?? dto);
        }

        public Task<bool> ActualizarAsync(
            int id,
            TDto dto,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(OperationResult);
        }

        public Task<bool> EliminarAsync(int id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(OperationResult);
        }
    }
}
