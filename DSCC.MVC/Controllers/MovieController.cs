using DSCC.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

namespace DSCC.MVC.Controllers
{
    public class MovieController : Controller
    {
        private const string BASE_URL = "http://ec2-107-22-147-166.compute-1.amazonaws.com/";  // The base URL of the API
        private readonly Uri ClientBasAddress = new Uri(BASE_URL);  // The base address for the API client
        private readonly HttpClient _client; // HTTP client for making API requests

        public MovieController()
        {
            _client = new HttpClient();
            _client.BaseAddress = ClientBasAddress;
        }

        private void HeaderClearing()   // Clear default HTTP request headers and set the request type to JSON
        {
            // Clearing default headers
            _client.DefaultRequestHeaders.Clear();

            // Define the request type of the data
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IActionResult> Index()
        {
            var movies = new List<Movie>(); // Initialize a list of movies
            HeaderClearing();

            // Send an HTTP GET request to retrieve all movies
            HttpResponseMessage httpResponseMessage = await _client.GetAsync("api/Movies");

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                // Read the response content and deserialize it to a list of movies
                var responseMessage = await httpResponseMessage.Content.ReadAsStringAsync();
                movies = JsonConvert.DeserializeObject<List<Movie>>(responseMessage);
            }
            return View(movies);
        }

        public async Task<ActionResult> Details(int id)
        {
            // Initialize a movie object
            Movie movie = new Movie();
            HeaderClearing();

            // Send an HTTP GET request to retrieve a specific movie by ID
            HttpResponseMessage httpResponseMessage = await _client.GetAsync($"api/Movies/{id}");

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                // Read the response content and deserialize it to a movie object
                string responseMessage = httpResponseMessage.Content.ReadAsStringAsync().Result;
                movie = JsonConvert.DeserializeObject<Movie>(responseMessage);
            }
            return View(movie);
        }

        // GET: Movie/Create
        public ActionResult Create()
        {
            Movie movie = new Movie();  // Initialize a new movie object for creating a new movie
            HeaderClearing();
            return View(movie);
        }

        // POST: Movie/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Movie movie)
        {
            if (ModelState.IsValid)
            {
                // Serialize the movie object to JSON and create a string content for the request
                string createGenreInfo = JsonConvert.SerializeObject(movie);
                StringContent stringContentInfo = new StringContent(createGenreInfo, Encoding.UTF8, "application/json");

                // Send an HTTP POST request to create a new movie.
                HttpResponseMessage createHttpResponseMessage = _client.PostAsync(_client.BaseAddress + "api/Movies", stringContentInfo).Result;
                if (createHttpResponseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(movie);
        }

        // GET: Movie/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            Movie movie = new Movie();  // Initialize a movie object for editing
            HeaderClearing();

            // Send an HTTP GET request to retrieve a specific movie by ID for editing
            HttpResponseMessage httpResponseMessage = await _client.GetAsync($"api/Movies/{id}");

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                // Read the response content and deserialize it to a movie object
                string responseMessage = httpResponseMessage.Content.ReadAsStringAsync().Result;
                movie = JsonConvert.DeserializeObject<Movie>(responseMessage);
            }

            return View(movie);
        }

        // POST: Subject/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Movie movie)
        {
            if (ModelState.IsValid)
            {
                // Serialize the movie object to JSON and create a string content for the request
                string createSubjectInfo = JsonConvert.SerializeObject(movie);
                StringContent stringContentInfo = new StringContent(createSubjectInfo, Encoding.UTF8, "application/json");

                // Send an HTTP PUT request to update a specific movie by ID
                HttpResponseMessage editHttpResponseMessage = _client.PutAsync(_client.BaseAddress + $"api/Movies/{id}", stringContentInfo).Result;
                if (editHttpResponseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(movie);
        }

        // GET: Subject/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            Movie movie = new Movie();  // Initialize a movie object for deleting
            HeaderClearing();

            // Send an HTTP GET request to retrieve a specific movie by ID for deletion
            HttpResponseMessage httpResponseMessage = await _client.GetAsync($"api/Movies/{id}");

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                // Read the response content and deserialize it to a movie object
                string responseMessage = httpResponseMessage.Content.ReadAsStringAsync().Result;
                movie = JsonConvert.DeserializeObject<Movie>(responseMessage);
            }
            return View(movie);
        }

        // POST: Subject/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Movie subject)
        {
            // Send an HTTP DELETE request to delete a specific movie by ID
            HttpResponseMessage deleteSubjectHttpResponseMessage = _client.DeleteAsync(_client.BaseAddress + $"api/Movies/{id}").Result;
            if (deleteSubjectHttpResponseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
