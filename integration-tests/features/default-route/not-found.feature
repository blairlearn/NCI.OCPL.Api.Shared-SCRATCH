Feature: Verify calls to unknown paths to fail for all verbs.

    Background:
        * url apiHost

    Scenario Outline: Verbs not requiring a request body result in an error message when sent to an invalid route.
        verb '<verb>'

        Given path 'foo'
        When method <verb>
        Then status 404
        And match response == {"Message":"Invalid Route."}

        Examples:
            | verb    |
            | DELETE  |
            | GET     |
            | OPTIONS |


    Scenario Outline: Verbs not returning a response body result in an error message when sent to an invalid route.
        verb '<verb>'

        Given path 'foo'
        When method <verb>
        Then status 404

        Examples:
            | verb    |
            | HEAD    |


    Scenario Outline: Verbs requiring a request body result in an error message when sent to an invalid route.
        verb '<verb>'

        Given path 'foo'
        And request { name: 'value' }
        When method <verb>
        Then status 404
        And match response == {"Message":"Invalid Route."}

        Examples:
            | verb    |
            | PATCH   |
            | POST    |
            | PUT     |
