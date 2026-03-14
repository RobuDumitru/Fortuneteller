using Microsoft.AspNetCore.Mvc;
using Fortuneteller.Models;

namespace Fortuneteller.Controllers
{
    public class TestController : Controller
    {
        static List<Question> questions = new List<Question>()
        {
            new Question{Text="Îmi place să socializez.",Trait="Extraversion"},
            new Question{Text="Am imaginație bogată.",Trait="Openness"},
            new Question{Text="Sunt organizat.",Trait="Conscientiousness"},
            new Question{Text="Sunt empatic.",Trait="Agreeableness"},
            new Question{Text="Mă stresez ușor.",Trait="Neuroticism"}
        };

        static Dictionary<string, int> score = new()
        {
            {"Extraversion",0},
            {"Openness",0},
            {"Conscientiousness",0},
            {"Agreeableness",0},
            {"Neuroticism",0}
        };

        static int index = 0;

        public IActionResult Index()
        {
            if (index >= questions.Count)
                return RedirectToAction("Result");

            ViewBag.Question = questions[index].Text;
            ViewBag.Progress = index * 20;
            return View();
        }

        [HttpPost]
        public IActionResult Answer(int value)
        {
            score[questions[index].Trait] += value;
            index++;
            return RedirectToAction("Index");
        }

        public IActionResult Result()
        {
            return View(score);
        }
    }
}