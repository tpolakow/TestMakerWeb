using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestMakerWeb.Data
{
  public class DbSeeder
  {
    #region Metody publiczne
    public static void Seed(ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
      //Utwórz domyślnych użytkowników (jeśli nie ma żadnych)
      if (!dbContext.Users.Any())
        CreateUsers(dbContext, roleManager, userManager)
          .GetAwaiter()
          .GetResult();
      //Utwórz domyślne quizy (jeśli nie ma żadnych) wraz z pytaniami i odpowiedziami
      if (!dbContext.Quizzes.Any())
        CreateQuizzes(dbContext);
    }
    #endregion
    #region Metody generujące
    private static async Task CreateUsers(ApplicationDbContext dbContext,
      RoleManager<IdentityRole> roleManager,
      UserManager<ApplicationUser> userManager)
    {
      //Zmienne lokalne
      DateTime createdDate = DateTime.Now;
      DateTime lastModifiedDate = DateTime.Now;

      string role_Administrator = "Administrator";
      string role_RegisteredUser = "ZarejestrowanyUżytkownik";

      //Utworzenie ról (jeśli jeszcze nie istnieją)
      if (!await roleManager.RoleExistsAsync(role_Administrator))
      {
        await roleManager.CreateAsync(new IdentityRole(role_Administrator));
      }
      if (!await roleManager.RoleExistsAsync(role_RegisteredUser))
      {
        await roleManager.CreateAsync(new IdentityRole(role_RegisteredUser));
      }

      //Utwórz konto użytkownika "Admin" (jeśli jeszcze nie istnieje)
      var user_Admin = new ApplicationUser()
      {
        SecurityStamp = Guid.NewGuid().ToString(),
        UserName = "Admin",
        Email = "jamaszi@gmail.com",
        DisplayName = "TomaH",
        CreatedDate = createdDate,
        LastModifiedDate = lastModifiedDate
      };

      //Wstaw do bazy danych użytkownika Admin i przypisz mu obie utworzone role
      if(await userManager.FindByNameAsync(user_Admin.UserName) == null)
      {
        await userManager.CreateAsync(user_Admin, "Pass4Admin");
        await userManager.AddToRoleAsync(user_Admin, role_RegisteredUser);
        await userManager.AddToRoleAsync(user_Admin, role_Administrator);
        //Usuń blokadę i potwierdz email
        user_Admin.EmailConfirmed = true;
        user_Admin.LockoutEnabled = false;
      }

#if DEBUG
      //Utwórz przykładowe konta zarejestrowanych użytkowników (jeśli jeszcze nie istnieją)
      var user_Tomasz = new ApplicationUser()
      {
        SecurityStamp = Guid.NewGuid().ToString(),
        UserName = "Tomasz",
        Email = "tomasz@testmaker.com",
        CreatedDate = createdDate,
        LastModifiedDate = lastModifiedDate
      };

      var user_Kasia = new ApplicationUser()
      {
        SecurityStamp = Guid.NewGuid().ToString(),
        UserName = "Kasia",
        Email = "kasia@testmaker.com",
        DisplayName = "Katenade",
        CreatedDate = createdDate,
        LastModifiedDate = lastModifiedDate
      };

      var user_Lechu = new ApplicationUser()
      {
        SecurityStamp = Guid.NewGuid().ToString(),
        UserName = "Lechu",
        Email = "lennyrogal@gmail.com",
        DisplayName = "lennyrogal",
        CreatedDate = createdDate,
        LastModifiedDate = lastModifiedDate
      };

      //Wstaw przykładowych użytkowników do bazy danych i przypisz im rolę zarejestrowanych
      if (await userManager.FindByNameAsync(user_Tomasz.UserName) == null)
      {
        await userManager.CreateAsync(user_Tomasz, "Pass4Tomasz");
        await userManager.AddToRoleAsync(user_Tomasz, role_RegisteredUser);
        //Usuń blokadę i potwierdz email
        user_Tomasz.EmailConfirmed = true;
        user_Tomasz.LockoutEnabled = false;
      }
      if (await userManager.FindByNameAsync(user_Kasia.UserName) == null)
      {
        await userManager.CreateAsync(user_Kasia, "Pass4Kasia");
        await userManager.AddToRoleAsync(user_Kasia, role_RegisteredUser);
        //Usuń blokadę i potwierdz email
        user_Kasia.EmailConfirmed = true;
        user_Kasia.LockoutEnabled = false;
      }
      if (await userManager.FindByNameAsync(user_Lechu.UserName) == null)
      {
        await userManager.CreateAsync(user_Lechu, "Pass4Lechu");
        await userManager.AddToRoleAsync(user_Lechu, role_RegisteredUser);
        //Usuń blokadę i potwierdz email
        user_Lechu.EmailConfirmed = true;
        user_Lechu.LockoutEnabled = false;
      }
#endif

      await dbContext.SaveChangesAsync();
    }

    private static void CreateQuizzes(ApplicationDbContext dbContext)
    {
      //Zmienne lokalne
      DateTime createdDate = DateTime.Now;
      DateTime lastModifiedDate = DateTime.Now;

      //Pobierz użytkownika Admin, bo użyjemy go jako domyślnego autora
      var authorId = dbContext.Users.Where(u => u.UserName == "Admin").FirstOrDefault().Id;

#if DEBUG
      //Utwórz 47 przykładowych quizów z automatycznie wygenerowanymi danymi
      //(włączając w to pytania, odpowiedzi i wyniki
      var num = 47;
      for (int i = 1; i <= num; i++)
      {
        CreateSampleQuiz(dbContext, i, authorId, num - i, 3, 3, 3, createdDate.AddDays(-num));
      }
#endif

      //Utwórz jeszcze 3 quizy z lepszymi danymi opisowymi
      //(pytania, odpowiedzi i wyniki dodamy później)
      EntityEntry<Quiz> e1 = dbContext.Quizzes.Add(new Quiz()
      {
        UserId = authorId,
        Title = "Jesteś po Jasnej czy po Ciemnej stronie Mocy?",
        Description = "Test osobowości bazujący na Gwiezdnych Wojnach",
        Text = @"Mądrze wybrać musisz, młody padawanie: " +
                "ten test sprawdzi, czy twoja wola jest na tyle silna, " +
                "aby pasować do zasad Jasnej strony Mocy, czy też " +
                "jesteś skazany na skuszenie się na Ciemną stronę. " +
                "Jeśli chcesz zostać prawdziwym JEDI, nie możesz pominąć takiej szansy!",
        ViewCount = 2343,
        CreatedDate = createdDate,
        LastModifiedDate = lastModifiedDate
      });

      EntityEntry<Quiz> e2 = dbContext.Quizzes.Add(new Quiz()
      {
        UserId = authorId,
        Title = "Pokolenie X, Y czy Z?",
        Description = "Dowiedz się, do której dekady najlepiej pasujesz",
        Text = @"Czy czujesz się dobrze w swoim pokoleniu? " +
                "W którym roku powinieneś się urodzić?" +
                "Oto kilka pytań które pozwolą Ci się o tym dowiedzieć!",
        ViewCount = 4180,
        CreatedDate = createdDate,
        LastModifiedDate = lastModifiedDate
      });

      EntityEntry<Quiz> e3 = dbContext.Quizzes.Add(new Quiz()
      {
        UserId = authorId,
        Title = "Którą postacią z Shingeki No Kyojin jesteś?",
        Description = "Test osobowości bazujący na Ataku Tytanów",
        Text = @"Czy niestrydzenie szukasz zemsty jak Eren? " +
                "Czy będziesz się narażać, aby chronić swoich przyjaciół jak Mikasa? " +
                "Czy ufasz swoim umiejętnościom walki jak Levi? " +
                "Czy raczej wolisz polegać na strategiach i taktyce jak Arwin? " +
                "Odkryj prawdziwego siebie dzięki temu testowi osobowości!",
        ViewCount = 5203,
        CreatedDate = createdDate,
        LastModifiedDate = lastModifiedDate
      });

      //Zapisz zmiany w bazie danych
      dbContext.SaveChanges();
    }
    #endregion
    #region Metody pomocnicze
    ///<summary>
    ///Tworzy przykładowy quiz i dodaje go do bazy danych
    ///razem z przykładowym zestawem pytań, odpowiedzi i wyników
    ///</summary>
    ///<param name="userId">identyfikator autora</param>
    ///<param name="id">identyfikator quizu</param>
    ///<param name="createdDate">data utworzenia quizu</param>
    private static void CreateSampleQuiz(ApplicationDbContext dbContext, int num, string authorId, int viewCount, 
      int numberOfQuestions, int numberOfAnswersPerQuestion, int numberOfResults, DateTime createdDate)
    {
      var quiz = new Quiz()
      {
        UserId = authorId,
        Title = String.Format("Tytuł quizu {0}", num),
        Description = String.Format("To jest przykładowy opis quizu {0}.", num),
        Text = "To jest przykładowy quiz utworzony przez klasę DbSeeder w celach testowych. " +
               "Wszystkie pytania, odpowiedzi i wyniki również zostały wygenerowane automatycznie.",
        ViewCount = viewCount,
        CreatedDate = createdDate,
        LastModifiedDate = createdDate
      };
      dbContext.Quizzes.Add(quiz);
      dbContext.SaveChanges();

      for(int i = 0; i < numberOfQuestions; i++)
      {
        var question = new Question()
        {
          QuizId = quiz.Id,
          Text = "To jest przykładowe pytanie utworzone przez klasę DbSeeder w celach testowych. " +
                 "Wszystkie odpowiedzi i pytania również są wygenerowane automatycznie.",
          CreatedDate = createdDate,
          LastModifiedDate = createdDate
        };
        dbContext.Questions.Add(question);
        dbContext.SaveChanges();

        for(int i2 = 0; i2 < numberOfAnswersPerQuestion; i2++)
        {
          var e2 = dbContext.Answers.Add(new Answer()
          {
            QuestionId = question.Id,
            Text = "To jest przykładowa odpowiedź utworzona przez klasę DbContext w celach testowych.",
            Value = i2,
            CreatedDate = createdDate,
            LastModifiedDate = createdDate
          });
        }
      }

      for(int i = 0; i < numberOfResults; i++)
      {
        dbContext.Results.Add(new Result()
        {
          QuizId = quiz.Id,
          Text = "To jest przykładowy wynik utworzony przez klasę DbSeeder w celach testowych.",
          MinValue = 0,
          //wartość maksymalna powinna być równa iloczynowi liczby
          //odpowiedzi i maksymalnej wartości odpowiedzi
          MaxValue = numberOfAnswersPerQuestion * 2,
          CreatedDate = createdDate,
          LastModifiedDate = createdDate
        });
      }
      dbContext.SaveChanges();
    }
    #endregion
  }
}
