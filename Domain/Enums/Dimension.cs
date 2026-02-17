namespace AsyncWarehouse.Domain.Enums;

/// <summary>
/// Dimensions of the furniture item.
/// </summary>
/// <param name="Length">Length of the furniture item in centimeters.</param>
/// <param name="Width">Width of the furniture item in centimeters.</param>
/// <param name="Height">Height of the furniture item in centimeters.</param>
public record Demension(float Length, float Width, float Height);