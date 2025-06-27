namespace task04;

public class Fighter : ISpaceship
{
    public int Speed => 100;
    public int FirePower => 50;
    public void MoveForward() => Console.WriteLine($"Истребитель движется быстро! Со скоростью {Speed}");
    public void Rotate(int angle) => Console.WriteLine($"Истребитель быстро поворачивается на {angle}°");
    public void Fire() => Console.WriteLine($"Истребитель стреляет быстрыми выстрелами!Его мощность выстрела составляет: {FirePower}");
}