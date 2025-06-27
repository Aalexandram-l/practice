namespace task04;

public class Cruiser : ISpaceship
{
    public int Speed => 50;
    public int FirePower => 100;
    public void MoveForward() => Console.WriteLine($"Крейсер движется. Со скоростью {Speed}");
    public void Rotate(int angle) => Console.WriteLine($"Крейсер поворачивается на {angle}°");
    public void Fire() => Console.WriteLine($"Крейсер стреляет мощными ракетами!Его мощность выстрела составляет: {FirePower}");
}