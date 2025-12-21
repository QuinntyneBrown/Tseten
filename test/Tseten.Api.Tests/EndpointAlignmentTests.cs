// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Tseten.Api.Controllers;

namespace Tseten.Api.Tests;

/// <summary>
/// Tests to verify that backend endpoints align with frontend HTTP service calls.
///
/// Frontend Service Calls (from Angular app):
/// 1. AuthService.login():  POST ${baseUrl}/api/user/token
/// 2. AuthService.tryToLogin(): GET ${baseUrl}/api/user/current
/// 3. RequirementsList.loadRequirements(): GET ${baseUrl}/api/softwarerequirements
///
/// These tests verify that the corresponding controller actions exist.
/// </summary>
public class EndpointAlignmentTests
{
    [Fact]
    public void UserController_ShouldHaveTokenEndpoint()
    {
        // Frontend calls: POST ${baseUrl}/api/user/token
        var controllerType = typeof(UserController);
        var method = controllerType.GetMethod("AuthenticateAsync");

        method.Should().NotBeNull("Frontend auth.service.ts calls POST /api/user/token");

        var httpPostAttribute = method!.GetCustomAttributes(typeof(HttpPostAttribute), false)
            .Cast<HttpPostAttribute>().FirstOrDefault();

        httpPostAttribute.Should().NotBeNull();
        httpPostAttribute!.Template.Should().Be("token");
    }

    [Fact]
    public void UserController_ShouldHaveCurrentEndpoint()
    {
        // Frontend calls: GET ${baseUrl}/api/user/current
        var controllerType = typeof(UserController);
        var method = controllerType.GetMethod("GetCurrentAsync");

        method.Should().NotBeNull("Frontend auth.service.ts calls GET /api/user/current");

        var httpGetAttribute = method!.GetCustomAttributes(typeof(HttpGetAttribute), false)
            .Cast<HttpGetAttribute>().FirstOrDefault();

        httpGetAttribute.Should().NotBeNull();
        httpGetAttribute!.Template.Should().Be("current");
    }

    [Fact]
    public void SoftwareRequirementsController_ShouldHaveGetEndpoint()
    {
        // Frontend calls: GET ${baseUrl}/api/softwarerequirements
        var controllerType = typeof(SoftwareRequirementsController);
        var method = controllerType.GetMethod("GetAsync");

        method.Should().NotBeNull("Frontend requirements-list.ts calls GET /api/softwarerequirements");

        var httpGetAttribute = method!.GetCustomAttributes(typeof(HttpGetAttribute), false)
            .Cast<HttpGetAttribute>().FirstOrDefault();

        httpGetAttribute.Should().NotBeNull();
    }

    [Fact]
    public void UserController_ShouldHaveCorrectRoutePrefix()
    {
        // Frontend expects: /api/user/*
        var controllerType = typeof(UserController);
        var routeAttribute = controllerType.GetCustomAttributes(typeof(RouteAttribute), false)
            .Cast<RouteAttribute>().FirstOrDefault();

        routeAttribute.Should().NotBeNull();
        routeAttribute!.Template.Should().Be("api/user");
    }

    [Fact]
    public void SoftwareRequirementsController_ShouldHaveCorrectRoutePrefix()
    {
        // Frontend expects: /api/softwarerequirements/*
        var controllerType = typeof(SoftwareRequirementsController);
        var routeAttribute = controllerType.GetCustomAttributes(typeof(RouteAttribute), false)
            .Cast<RouteAttribute>().FirstOrDefault();

        routeAttribute.Should().NotBeNull();
        routeAttribute!.Template.Should().Be("api/softwarerequirements");
    }

    [Fact]
    public void TagsController_ShouldExist()
    {
        // Backend provides tag management endpoints
        var controllerType = typeof(TagsController);
        controllerType.Should().NotBeNull();

        var routeAttribute = controllerType.GetCustomAttributes(typeof(RouteAttribute), false)
            .Cast<RouteAttribute>().FirstOrDefault();

        routeAttribute.Should().NotBeNull();
        routeAttribute!.Template.Should().Be("api/tags");
    }

    /// <summary>
    /// Documents all frontend endpoints that should exist on the backend.
    /// This test serves as living documentation of the frontend/backend contract.
    /// </summary>
    [Theory]
    [InlineData("POST", "/api/user/token", "Login/Authenticate")]
    [InlineData("GET", "/api/user/current", "Get current logged-in user")]
    [InlineData("GET", "/api/softwarerequirements", "Get all software requirements")]
    public void FrontendEndpoints_ShouldBeDocumented(string method, string path, string description)
    {
        // This test documents the frontend/backend contract
        // If this test passes, it means the documented endpoints are expected
        method.Should().NotBeNullOrEmpty();
        path.Should().StartWith("/api/");
        description.Should().NotBeNullOrEmpty();
    }
}
