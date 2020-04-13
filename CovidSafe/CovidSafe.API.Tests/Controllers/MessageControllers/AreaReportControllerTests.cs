﻿using System.Threading;

using CovidSafe.API.Controllers.MessageControllers;
using CovidSafe.DAL.Services;
using CovidSafe.Entities.Protos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CovidSafe.API.Tests.Controllers.MessageControllers
{
    /// <summary>
    /// Unit tests for the <see cref="AreaReportController"/> class
    /// </summary>
    [TestClass]
    public class AreaReportControllerTests
    {
        /// <summary>
        /// Test <see cref="HttpContext"/> instance
        /// </summary>
        private Mock<HttpContext> _context;
        /// <summary>
        /// Test <see cref="ListController"/> instance
        /// </summary>
        private AreaReportController _controller;
        /// <summary>
        /// Mock <see cref="IMessageService"/>
        /// </summary>
        private Mock<IMessageService> _service;

        /// <summary>
        /// Creates a new <see cref="AreaReportControllerTests"/> instance
        /// </summary>
        public AreaReportControllerTests()
        {
            // Configure service object
            this._service = new Mock<IMessageService>();

            // Create HttpContext mock
            this._context = new Mock<HttpContext>();
            ActionContext actionContext = new ActionContext
            {
                HttpContext = this._context.Object,
                RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
                ActionDescriptor = new ControllerActionDescriptor()
            };

            // Configure controller
            this._controller = new AreaReportController(this._service.Object);
            this._controller.ControllerContext = new ControllerContext(actionContext);
        }

        /// <summary>
        /// <see cref="AreaReportController.PutAsync(AreaMatch, CancellationToken)"/> 
        /// returns <see cref="BadRequestResult"/> when no <see cref="Area"/> objects are provided 
        /// with request
        /// </summary>
        [TestMethod]
        public void PutAsync_BadResultRequestWithNoAreas()
        {
            // Arrange
            AreaMatch requestObj = new AreaMatch
            {
                UserMessage = "This is a message"
            };

            // Act
            ActionResult controllerResponse = this._controller
                .PutAsync(requestObj, CancellationToken.None)
                .Result;

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse, typeof(BadRequestResult));
        }

        /// <summary>
        /// <see cref="AreaReportController.PutAsync(AreaMatch, CancellationToken)"/> 
        /// returns <see cref="BadRequestResult"/> when no user message is specified
        /// </summary>
        [TestMethod]
        public void PutAsync_BadResultRequestWithNoUserMessage()
        {
            // Arrange
            AreaMatch requestObj = new AreaMatch();
            requestObj.Areas.Add(new Area
            {
                BeginTime = 0,
                EndTime = 1,
                Location = new Location
                {
                    Latitude = 10.1234,
                    Longitude = 10.1234
                },
                RadiusMeters = 100
            });

            // Act
            ActionResult controllerResponse = this._controller
                .PutAsync(requestObj, CancellationToken.None)
                .Result;

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse, typeof(BadRequestResult));
        }

        /// <summary>
        /// <see cref="AreaReportController.PutAsync(AreaMatch, CancellationToken)"/> 
        /// returns <see cref="OkResult"/> with valid input data
        /// </summary>
        [TestMethod]
        public void PutAsync_OkResultWithValidInputs()
        {
            // Arrange
            AreaMatch requestObj = new AreaMatch
            {
                UserMessage = "User message content"
            };
            requestObj.Areas.Add(new Area
            {
                BeginTime = 0,
                EndTime = 1,
                Location = new Location
                {
                    Latitude = 10.1234,
                    Longitude = 10.1234
                },
                RadiusMeters = 100
            });

            // Act
            ActionResult controllerResponse = this._controller
                .PutAsync(requestObj, CancellationToken.None)
                .Result;

            // Assert
            Assert.IsNotNull(controllerResponse);
            Assert.IsInstanceOfType(controllerResponse, typeof(OkResult));
        }
    }
}
