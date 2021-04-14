Feature: Verify use of the default route doesn't prevent valid routes from working.

    Background:
        * url apiHost


    Scenario Outline: Verbs not requiring a request body receive the correct response when sent to a valid route.
        verb '<verb>'

        Given path 'valid-route'
        When method <verb>
        Then status 200
        And match response == "Valid Route."

        Examples:
            | verb    |
            | DELETE  |
            | GET     |
            | OPTIONS |


    Scenario Outline: Verbs not returning a response body receive the correct response when sent to a valid route.
        verb '<verb>'

        Given path 'valid-route'
        When method <verb>
        Then status 200

        Examples:
            | verb    |
            | HEAD    |


    Scenario Outline: Verbs requiring a request body receive the correct response when sent to a valid route.
        verb '<verb>'

        Given path 'valid-route'
        And request { name: 'value' }
        When method <verb>
        Then status 200
        And match response == "Valid Route."

        Examples:
            | verb    |
            | PATCH   |
            | POST    |
            | PUT     |
