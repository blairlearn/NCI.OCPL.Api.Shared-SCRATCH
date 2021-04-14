Feature: Verify requesting the swagger document works.

    Background:
        * url apiHost

    Scenario: Retrieve the swagger document.
        * configure followRedirects = false

        * def reasonableLength = function(content) { return content.length > 1000; }

        Given path 'index.html'
        When method GET
        Then status 200
        And match header Content-Type == 'text/html; charset=utf-8'
        And assert reasonableLength(response)

    Scenario Outline: Requests for the root document using verbs which do not require a request body redirects to the swagger document.
        verb '<verb>'
        * configure followRedirects = false

        Given path '/'
        When method <verb>
        Then status 302
        And match header Location == '/index.html'

        Examples:
            | verb    |
            | DELETE  |
            | GET     |
            | OPTIONS |
            | HEAD    |


    Scenario Outline: Requests for the root document using verbs which require a request body redirect to the swagger document.
        verb '<verb>'
        * configure followRedirects = false

        Given path '/'
        And request { name: 'value' }
        When method <verb>
        Then status 302
        And match header Location == '/index.html'

        Examples:
            | verb    |
            | PATCH   |
            | POST    |
            | PUT     |
