﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Funemployment.Data;
using Funemployment.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Funemployment.Controllers
{
    public class BehavioralController : Controller
    {
        private FunemploymentDbContext _context;

        public BehavioralController(FunemploymentDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all behavioral questions from the API
        /// </summary>
        /// <returns>a list of behavioral questions to the view or not found</returns>
        public async Task<IActionResult> Index()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://funemploymentapi.azurewebsites.net");

                var response = client.GetAsync("/api/behavior/").Result;

                if (response.EnsureSuccessStatusCode().IsSuccessStatusCode)
                {
                    var stringResult = await response.Content.ReadAsStringAsync();

                    var obj = JsonConvert.DeserializeObject<List<BehaviorQuestion>>(stringResult);
                    return View(obj);
                }
                return NotFound();
            }
        }

        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Answer answer)
        {
            await _context.AnswerTable.AddAsync(answer);
            await _context.SaveChangesAsync();
            return View();
        }

        /// <summary>
        /// gets data for exactly one behavioral questions
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetOneBQ(int? id)
        {
            if (id.HasValue)
            {
                return View(await OneBQViewModel.FromIDAsync(id.Value, _context));
            }
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> CreateAnswer(int? id)
        {
            if (id.HasValue)
            {
                return View(id);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAnswer(Answer answer)
        {
            
            await _context.AnswerTable.AddAsync(answer);
            await _context.SaveChangesAsync();
            return View();
        }
    }
}