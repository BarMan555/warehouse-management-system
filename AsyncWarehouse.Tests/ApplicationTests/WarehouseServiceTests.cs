using AsyncWarehouse.Application;
using AsyncWarehouse.Application.AutoMapper;
using AsyncWarehouse.Application.Interfaces;
using AsyncWarehouse.Domain.Models;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;

namespace AsyncWarehouse.Tests.ApplicationTests;

public class WarehouseServiceTests
{
    private readonly IMapper _mapper;

    public WarehouseServiceTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        },
        LoggerFactory.Create(b => { }));
        _mapper = configuration.CreateMapper();
    }

    [Fact]
    public async Task GetPalletAsync_WhenPalletExists_ReturnsPalletGetDto()
    {
        // Arrange
        var palletId = Guid.NewGuid();
        var pallet = new Pallet(100f) { Id = palletId }; 

        var mockRepository = new Mock<IPalletRepository>();

        mockRepository
            .Setup(repo => repo.GetByIdAsync(palletId))
            .ReturnsAsync(pallet);

        var warehouseService = new WarehouseService(mockRepository.Object, _mapper);

        // Act
        var result = await warehouseService.GetPalletAsync(palletId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(palletId, result.Id);
        Assert.Equal(100f, result.MaxCapacity);
    }

    [Fact]
    public async Task GetPalletAsync_WhenPalletDoesNotExist_ThrowsArgumentException()
    {
        // Arrange
        var invalidPalletId = Guid.NewGuid();
        var mockRepository = new Mock<IPalletRepository>();

        mockRepository
            .Setup(repo => repo.GetByIdAsync(invalidPalletId))
            .ReturnsAsync((Pallet?)null);

        var warehouseService = new WarehouseService(mockRepository.Object, _mapper);

        // Act
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => 
            warehouseService.GetPalletAsync(invalidPalletId));
        
        // Assert
        Assert.Contains($"Pallet with id {invalidPalletId} not found", exception.Message);
    }
}