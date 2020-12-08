# Arrowhead Client Library C#
A client library for creating Service Providers and Consumers for [Arrowhead](https://www.arrowhead.eu) written in C#.
The intention of this library is to make it eaiser for developers to register and orchestrate services by providing an interface against the [Core Systems](https://github.com/arrowhead-f/core-java-spring).

An example of how to use the library with a basic Producer and Consumer can be found [here](https://github.com/97gushan/arrowhead-client).

## Developement Dependencies

## Runtime Dependencies
To run a program with the library the mandatory core Arrowhead systems must be running and their ip addresses and ports must be configured correctly.
Client certificates must also be provided, a guide on how to do this can be found [here](https://github.com/arrowhead-f/core-java-spring/blob/master/documentation/certificates/create_client_certificate.pdf).


## Limitations
This project is in a very early development state and this it has some major limitations. This includes but is not limited to:
* This library only connects to the three mandatory core systems
* No existing test suite
* No logging system
* Lacking error handling
