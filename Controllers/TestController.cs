using Microsoft.AspNetCore.Mvc;
using Fortuneteller.Models;
using System.Text.Json;

namespace Fortuneteller.Controllers
{
    public class TestController : Controller
    {
        static readonly List<Question> Questions = new()
        {
            new Question{Text="Îmi place să fiu în centrul atenției.", Trait="Extraversion"},
            new Question{Text="Prefer activități liniștite.", Trait="Extraversion"},
            new Question{Text="Îmi place să socializez des.", Trait="Extraversion"},
            new Question{Text="Sunt plin de energie.", Trait="Extraversion"},
            new Question{Text="Vorbesc mult în grup.", Trait="Extraversion"},
            new Question{Text="Am imaginație bogată.", Trait="Openness"},
            new Question{Text="Îmi place arta și muzica.", Trait="Openness"},
            new Question{Text="Sunt curios din punct de vedere intelectual.", Trait="Openness"},
            new Question{Text="Îmi plac ideile abstracte.", Trait="Openness"},
            new Question{Text="Încerc lucruri noi cu plăcere.", Trait="Openness"},
            new Question{Text="Îmi planific activitățile cu atenție.", Trait="Conscientiousness"},
            new Question{Text="Respect întotdeauna termenele.", Trait="Conscientiousness"},
            new Question{Text="Sunt o persoană organizată.", Trait="Conscientiousness"},
            new Question{Text="Muncesc disciplinat și eficient.", Trait="Conscientiousness"},
            new Question{Text="Îmi controlez impulsurile.", Trait="Conscientiousness"},
            new Question{Text="Îi ajut pe ceilalți cu plăcere.", Trait="Agreeableness"},
            new Question{Text="Sunt empatic față de problemele altora.", Trait="Agreeableness"},
            new Question{Text="Evit conflictele.", Trait="Agreeableness"},
            new Question{Text="Am încredere în oameni.", Trait="Agreeableness"},
            new Question{Text="Sunt o persoană cooperantă.", Trait="Agreeableness"},
            new Question{Text="Mă stresez ușor.", Trait="Neuroticism"},
            new Question{Text="Mă îngrijorez frecvent.", Trait="Neuroticism"},
            new Question{Text="Sunt sensibil emoțional.", Trait="Neuroticism"},
            new Question{Text="Am schimbări de dispoziție dese.", Trait="Neuroticism"},
            new Question{Text="Mă simt tensionat în situații dificile.", Trait="Neuroticism"},
        };

        private Dictionary<string, int> GetScore()
        {
            var json = HttpContext.Session.GetString("score");
            if (json == null)
                return new Dictionary<string, int>
                {
                    ["Extraversion"] = 0,
                    ["Openness"] = 0,
                    ["Conscientiousness"] = 0,
                    ["Agreeableness"] = 0,
                    ["Neuroticism"] = 0
                };
            return JsonSerializer.Deserialize<Dictionary<string, int>>(json)!;
        }

        private void SaveScore(Dictionary<string, int> score)
            => HttpContext.Session.SetString("score", JsonSerializer.Serialize(score));

        public IActionResult Index()
        {
            int index = HttpContext.Session.GetInt32("index") ?? 0;

            if (index >= Questions.Count)
                return RedirectToAction("Result");

            ViewBag.Question = Questions[index].Text;
            ViewBag.Index = index + 1;
            ViewBag.Total = Questions.Count;
            ViewBag.Progress = (int)((double)index / Questions.Count * 100);
            return View();
        }

        [HttpPost]
        public IActionResult Answer(int value)
        {
            int index = HttpContext.Session.GetInt32("index") ?? 0;
            var score = GetScore();
            score[Questions[index].Trait] += value;
            SaveScore(score);
            HttpContext.Session.SetInt32("index", index + 1);
            return RedirectToAction("Index");
        }

        public IActionResult Result()
        {
            var score = GetScore();
            HttpContext.Session.Remove("index");
            HttpContext.Session.Remove("score");
            return View(score);
        }
    }
}