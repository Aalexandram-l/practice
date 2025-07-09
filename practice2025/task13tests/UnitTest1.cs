using Xunit;
using System;

public class JsonServiceTests
{
    [Fact]
    public void SerializeStudent_Test()
    {
        var student = new Student
        {
            FirstName = "Сергей",
            LastName = "Казаков",
            BirthDate = new DateTime(1985, 1, 24),
            Grades = new List<Subject>
            {
                new Subject { Name = "Математика", Grade = 5 },
                new Subject { Name = "Информатика", Grade = 4 }
            }
        };

        string json = JsonService.SerializeStudent(student);
        Assert.Contains("Сергей", json); 
        Assert.Contains("1985-01-24", json);
        Assert.Contains("Математика", json);
        Assert.Contains("5", json);
    }

    [Fact]
    public void DeserializeStudent_Test()
    {
        string json = @"{
            ""FirstName"": ""Сергей"",
            ""LastName"": ""Казаков"",
            ""BirthDate"": ""1985-01-24"",
            ""Grades"": [
                { ""Name"": ""Математика"", ""Grade"": 5 },
                { ""Name"": ""Информатика"", ""Grade"": 4 }
            ]
        }";

        Student student = JsonService.DeserializeStudent(json);
        Assert.Equal("Сергей", student.FirstName);
        Assert.Equal("Казаков", student.LastName);
        Assert.Equal(new DateTime(1985, 1, 24), student.BirthDate);
        Assert.Equal(2, student.Grades.Count);
        Assert.Equal("Математика", student.Grades[0].Name);
        Assert.Equal(5, student.Grades[0].Grade);
    }
}