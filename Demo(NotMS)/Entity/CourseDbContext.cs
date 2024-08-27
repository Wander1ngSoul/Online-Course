using OnlineCourse.Entity.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using OnlineCourse.Entity.Repositories;
using OnlineCourse.Services;

namespace OnlineCourse.Entity
{
    public class CourseDbContext : DbContext, IDbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public CourseDbContext(DbContextOptions<CourseDbContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>()
                .HasKey(c => new { c.UserId, c.CourseId });

            //modelBuilder.Entity<Cart>()
            //    .HasOne(c => c.User)
            //    .WithMany(u => u.Carts)
            //    .HasForeignKey(c => c.UserId);

            //modelBuilder.Entity<Cart>()
            //    .HasOne(c => c.Course)
            //    .WithMany(c => c.Carts)
            //    .HasForeignKey(c => c.CourseId);
            modelBuilder.Entity<User>().HasData(

                    new User
                    {
                        Id = 1,
                        Email = "test",
                        Password = UserService.HashPassword("test"),
                        FirstName = "Petrov",
                        LastName = "Petr",
                        Role = "Преподаватель",
                    },
                     new User
                     {
                         Id = 2,
                         Email = "student",
                         Password = UserService.HashPassword("student"),
                         FirstName = "Ivanov",
                         LastName = "Ivan",
                         Role = "Студент",
                     }
                    );

            modelBuilder.Entity<Course>().HasData(
        new Course { CourseId = 1, NameCourse = "Web - разработчик", Hours = 9, Cost = 12599, inforamtion = "Предложение ограничено и действует в течении недели", Img = "/img/Cource1.jpg" },
        new Course { CourseId = 2, NameCourse = "Мобильная разработка", Hours = 8, Cost = 10999, inforamtion = "Узнайте, как создавать мобильные приложения для iOS и Android", Img = "/img/Cource2.jpg" },
        new Course { CourseId = 3, NameCourse = "Python для начинающих", Hours = 4, Cost = 8999, inforamtion = "Научитесь программировать на Python с нуля", Img = "/img/Cource2.jpg" },
        new Course { CourseId = 4, NameCourse = "Java разработчик", Hours = 12 , Cost = 14999, inforamtion = "Углубленное обучение Java для создания профессиональных приложений", Img = "/img/Cource1.jpg" },
        new Course { CourseId = 5, NameCourse = "Аналитика данных", Hours = 10 , Cost = 13999, inforamtion = "Получите навыки анализа данных и машинного обучения", Img = "/img/Cource2.jpg" },
        new Course { CourseId = 6, NameCourse = "UX/UI дизайн", Hours = 8, Cost = 12999, inforamtion = "Изучение проектирования пользовательских интерфейсов и опытного дизайна", Img = "/img/Cource2.jpg" },
        new Course { CourseId = 7, NameCourse = "SQL и базы данных", Hours = 6, Cost = 9999, inforamtion = "Основы SQL и проектирование баз данных", Img = "/img/Cource2.jpg" },
        new Course { CourseId = 8, NameCourse = "Основы программирования", Hours = 3 , Cost = 6999, inforamtion = "Основные принципы программирования на языках Python и Java", Img = "/img/Cource2.jpg" },
        new Course { CourseId = 9, NameCourse = "Сетевая безопасность", Hours = 9 , Cost = 13599, inforamtion = "Защита компьютерных сетей от киберугроз и атак", Img = "/img/Cource2.jpg" },
        new Course { CourseId = 10, NameCourse = "Машинное обучение", Hours = 12, Cost = 15999, inforamtion = "Глубокое изучение алгоритмов машинного обучения и нейронных сетей", Img = "/img/Cource1.jpg" },
        new Course { CourseId = 11, NameCourse = "Системное администрирование", Hours = 9, Cost = 12999, inforamtion = "Настройка и управление сетевыми серверами и облачными ресурсами", Img = "/img/Cource1.jpg" },
        new Course { CourseId = 12, NameCourse = "Автоматизация тестирования", Hours = 6, Cost = 10999, inforamtion = "Изучение инструментов автоматизации тестирования и тестирования программного обеспечения", Img = "/img/Cource2.jpg" },
        new Course { CourseId = 13, NameCourse = "Frontend разработка", Hours = 6, Cost = 11999, inforamtion = "Разработка веб-интерфейсов с использованием HTML, CSS и JavaScript", Img = "/img/Cource1.jpg" },
        new Course { CourseId = 14, NameCourse = "DevOps инженер", Hours = 9, Cost = 13999, inforamtion = "Интеграция разработки и операций для улучшения процессов развертывания и обслуживания ПО", Img = "/img/Cource1.jpg" },
        new Course { CourseId = 15, NameCourse = "PHP разработчик", Hours = 8 , Cost = 11999, inforamtion = "Создание веб-приложений на PHP с использованием фреймворков", Img = "/img/Cource1.jpg" },
        new Course { CourseId = 16, NameCourse = "Администрирование баз данных", Hours = 6 , Cost = 9999, inforamtion = "Настройка и управление реляционными базами данных", Img = "/img/Cource1.jpg" },
        new Course { CourseId = 17, NameCourse = "Компьютерная графика", Hours = 6 , Cost = 10999, inforamtion = "Изучение основ компьютерной графики и создание графических приложений", Img = "/img/Cource1.jpg" },
        new Course { CourseId = 18, NameCourse = "Архитектура программного обеспечения", Hours = 9 , Cost = 12999, inforamtion = "Проектирование и разработка сложных программных систем", Img = "/img/Cource1.jpg" },
        new Course { CourseId = 19, NameCourse = "Бизнес-анализ", Hours = 6, Cost = 10999, inforamtion = "Анализ бизнес-процессов и формулирование требований к информационным системам", Img = "/img/Cource1.jpg" },
        new Course { CourseId = 20, NameCourse = "Сетевые технологии", Hours = 6, Cost = 9999, inforamtion = "Изучение сетевых протоколов и архитектур сетей", Img = "/img/Cource1.jpg" },
        new Course { CourseId = 21, NameCourse = "Интернет вещей (IoT)", Hours = 9, Cost = 13999, inforamtion = "Разработка приложений и устройств для интернета вещей", Img = "/img/Cource2.jpg" },
        new Course { CourseId = 22, NameCourse = "Системы искусственного интеллекта", Hours = 12, Cost = 15999, inforamtion = "Глубокое изучение алгоритмов машинного обучения и нейронных сетей", Img = "/img/Cource2.jpg" },
        new Course { CourseId = 23, NameCourse = "Кибербезопасность", Hours = 8 , Cost = 11999, inforamtion = "Обучение основам кибербезопасности и защите информации", Img = "/img/Cource2.jpg" },
        new Course { CourseId = 24, NameCourse = "Английский для IT", Hours = 6 , Cost = 8999, inforamtion = "Улучшите свой английский и станьте более успешным в IT", Img = "/img/Cource2.jpg" },
        new Course { CourseId = 25, NameCourse = "Фронтенд разработка", Hours =9 , Cost = 12999, inforamtion = "Изучение HTML, CSS и JavaScript для создания веб-интерфейсов", Img = "/img/Cource2.jpg" },
        new Course { CourseId = 26, NameCourse = "Игровая разработка", Hours = 12 , Cost = 16999, inforamtion = "Создание собственных компьютерных игр с использованием различных движков", Img = "/img/Cource2.jpg" },
        new Course { CourseId = 27, NameCourse = "Искусство анимации", Hours = 12 , Cost = 15999, inforamtion = "Погрузитесь в мир анимации и создайте собственные мультфильмы", Img = "/img/Cource2.jpg" },
        new Course { CourseId = 28, NameCourse = "Web - разработчик", Hours = 9 , Cost = 12599, inforamtion = "Предложение ограничено и действует в течении недели", Img = "/img/Cource1.jpg" },
        new Course { CourseId = 29, NameCourse = "Мобильная разработка", Hours = 6 , Cost = 10999, inforamtion = "Узнайте, как создавать мобильные приложения для iOS и Android", Img = "/img/Cource2.jpg" },
        new Course { CourseId = 30, NameCourse = "Python для начинающих", Hours = 4 , Cost = 8999, inforamtion = "Научитесь программировать на Python с нуля", Img = "/img/Cource2.jpg" },
        new Course { CourseId = 31, NameCourse = "Java разработчик", Hours = 12 , Cost = 14999, inforamtion = "Углубленное обучение Java для создания профессиональных приложений", Img = "/img/Cource1.jpg" },
        new Course { CourseId = 32, NameCourse = "Аналитика данных", Hours = 10 , Cost = 13999, inforamtion = "Получите навыки анализа данных и машинного обучения", Img = "/img/Cource2.jpg" },
        new Course { CourseId = 33, NameCourse = "UX/UI дизайн", Hours = 8 , Cost = 12999, inforamtion = "Изучение проектирования пользовательских интерфейсов и опытного дизайна", Img = "/img/Cource2.jpg" },
        new Course { CourseId = 34, NameCourse = "SQL и базы данных", Hours = 6, Cost = 9999, inforamtion = "Основы SQL и проектирование баз данных", Img = "/img/Cource2.jpg" },
        new Course { CourseId = 35, NameCourse = "Основы программирования", Hours = 3 , Cost = 6999, inforamtion = "Основные принципы программирования на языках Python и Java", Img = "/img/Cource1.jpg" },
        new Course { CourseId = 36, NameCourse = "Сетевая безопасность", Hours = 9, Cost = 13599, inforamtion = "Защита компьютерных сетей от киберугроз и атак", Img = "/img/Cource2.jpg" },
        new Course { CourseId = 37, NameCourse = "Машинное обучение", Hours = 12, Cost = 15999, inforamtion = "Глубокое изучение алгоритмов машинного обучения и нейронных сетей", Img = "/img/Cource1.jpg" },
        new Course { CourseId = 38, NameCourse = "Системное администрирование", Hours = 9, Cost = 12999, inforamtion = "Настройка и управление сетевыми серверами и облачными ресурсами", Img = "/img/Cource2.jpg" },
        new Course { CourseId = 39, NameCourse = "Автоматизация тестирования", Hours = 6, Cost = 10999, inforamtion = "Изучение инструментов автоматизации тестирования и тестирования программного обеспечения", Img = "/img/Cource1.jpg" },
        new Course { CourseId = 40, NameCourse = "Frontend разработка", Hours = 6 , Cost = 11999, inforamtion = "Разработка веб-интерфейсов с использованием HTML, CSS и JavaScript", Img = "/img/Cource2.jpg" },
        new Course { CourseId = 41, NameCourse = "DevOps инженер", Hours = 9 , Cost = 13999, inforamtion = "Интеграция разработки и операций для улучшения процессов развертывания и обслуживания ПО", Img = "/img/Cource1.jpg" },
        new Course { CourseId = 42, NameCourse = "PHP разработчик", Hours = 8, Cost = 11999, inforamtion = "Создание веб-приложений на PHP с использованием фреймворков", Img = "/img/Cource2.jpg" },
        new Course { CourseId = 43, NameCourse = "Администрирование баз данных", Hours = 6, Cost = 9999, inforamtion = "Настройка и управление реляционными базами данных", Img = "/img/Cource1.jpg" },
        new Course { CourseId = 44, NameCourse = "Компьютерная графика", Hours = 6, Cost = 10999, inforamtion = "Изучение основ компьютерной графики и создание графических приложений", Img = "/img/Cource2.jpg" },
        new Course { CourseId = 45, NameCourse = "Архитектура программного обеспечения", Hours = 9, Cost = 12999, inforamtion = "Проектирование и разработка сложных программных систем", Img = "/img/Cource1.jpg" },
        new Course { CourseId = 46, NameCourse = "Бизнес-анализ", Hours = 6 , Cost = 10999, inforamtion = "Анализ бизнес-процессов и формулирование требований к информационным системам", Img = "/img/Cource2.jpg" },
        new Course { CourseId = 47, NameCourse = "Сетевые технологии", Hours = 6, Cost = 9999, inforamtion = "Изучение сетевых протоколов и архитектур сетей", Img = "/img/Cource1.jpg" },
        new Course { CourseId = 48, NameCourse = "Интернет вещей (IoT)", Hours = 9, Cost = 13999, inforamtion = "Разработка приложений и устройств для интернета вещей", Img = "/img/Cource2.jpg" },
        new Course { CourseId = 49, NameCourse = "Системы искусственного интеллекта", Hours = 12, Cost = 15999, inforamtion = "Глубокое изучение алгоритмов машинного обучения и нейронных сетей", Img = "/img/Cource2.jpg" },
        new Course { CourseId = 50, NameCourse = "Кибербезопасность", Hours = 8, Cost = 11999, inforamtion = "Обучение основам кибербезопасности и защите информации", Img = "/img/Cource1.jpg" }

                    );
        }
    }
}
