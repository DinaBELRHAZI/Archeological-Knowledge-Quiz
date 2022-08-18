using ApiQuiz.Services;
using AppMobile;
using Microsoft.AspNetCore.Mvc;

namespace ApiQuiz.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizController : ControllerBase
    {
        private readonly QuizService _quizService;

        public QuizController(QuizService quizService) =>
            _quizService = quizService;

        [HttpGet]
        public async Task<List<quiz>> Get() =>
        await _quizService.GetAsync();


        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<quiz>> Get(string id)
        {
            var quizArcheo = await _quizService.GetAsync(id);

            if (quizArcheo is null)
            {
                return NotFound();
            }

            return quizArcheo;
        }


        [HttpPost]
        public async Task<IActionResult> Post(quiz newQuiz)
        {
            await _quizService.CreateAsync(newQuiz);

            return CreatedAtAction(nameof(Get), new { id = newQuiz.Id }, newQuiz);
        }



        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, quiz updatedQuiz)
        {
            var quizArcheo = await _quizService.GetAsync(id);

            if (quizArcheo is null)
            {
                return NotFound();
            }

            updatedQuiz.Id = quizArcheo.Id;

            await _quizService.UpdateAsync(id, updatedQuiz);

            return NoContent();
        }




        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var quizArcheo = await _quizService.GetAsync(id);

            if (quizArcheo is null)
            {
                return NotFound();
            }

            await _quizService.RemoveAsync(id);

            return NoContent();
        }

    }
}
