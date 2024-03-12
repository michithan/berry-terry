# Berry & Terry

Say hello to **Berry** & **Terry**

<img src="berry.svg" alt="berry" width="200"/>

**B**ot - _integrates your whole heterogenous development toolchain with ai, not just one commercial ecosystem_

**T**raining - _extract and transform project data to training data._

**Berry** & **Terry** is not just AI model & service like chatGPT.</br>
It's a e2e development AI integration & data extraction solution, that connects to various development tools.

## Introduction

### Decoupling AI Models, AI Integration and AI Fine-tuning

**Berry** decouples the AI model from AI integration into your development toolchain.</br>
**Terry** decouples the AI model from AI training data extraction of your development toolchain.</br>

Berry & Terry facilitates an adaptive setup where the AI component can be seamlessly replaced or updated.</br>
This ensures your development workflows always benefit from the latest advancements in AI,</br>
without the need for extensive reintegration efforts.

It also allows for more sophisticated integration into heterogenous development workflows,</br>
as these are likely spread across multiple tools, from multiple vendors.</br>
In contrast commercial AI solutions (like github copilot) integrate just to vendor specific ecosystems,</br>
therefore not providing the same level of integration and possible data extraction for fine tuning.

### Why Berry **&** Terry, not just Berry **or** Terry?

It might make sense to see Berry & Terry as separate projects,</br>
as they could be developed and maintained by different teams,</br>
since the AI module itself is the only interface that Berry need's to communicate with.</br>

On the other hand, both projects need to implement clients to interact with SCM platform's and development specific services,</br>
like ticketing systems, wikis, etc. that are specific to conversational AI.</br>

Putting Berry & Terry into one project allows to test the solution e2e by itself through [Dogfooding](https://en.wikipedia.org/wiki/Eating_your_own_dog_food),</br>
which is also perfect for Berry & Terry as robodogo's, since they love dogfood 😊.</br>

## Architecture

This section describes the project architecture.
The project is split into 3 main components:

```mermaid
C4Context
    title Berry & Terry Architecture

    Enterprise_Boundary(b0, "Software Development Project") {
        Person(p1, "AI Trainer")
        Person(p0, "Developer")

        BiRel(p0, c0, "uses")
        BiRel(p0, c1, "uses")
        BiRel(p0, c2, "uses")

        BiRel(p1, s2, "uses")

        System_Boundary(b1, "Berry & Terry") {
            System(s2, "Terry")
            System(s1, "Berry")

            BiRel(s1, m1, "calls")
            Rel(s2, m1, "fine-tunes")
            System_Boundary(b3, "AI Model") {
                Container(m1, "AI Model")
            }

            BiRel(s1, s0, "uses")
            Rel(s0, s2, "uses")
            System_Boundary(b2, "Shared lib") {
                System(s0, "Leash")
            }

        }

        BiRel(s0, c0, "integrates")
        BiRel(s0, c1, "integrates")
        BiRel(s0, c2, "integrates")
        System_Boundary(b4, "Developer Tools") {
            Container(c0, "SCM Platform")
            Container(c1, "Ticketing System")
            Container(c2, "Wiki")
        }
    }

```

### Leash

Leash is the shared library for Berry & Terry.
It Implements the integration to various development tools.
It provides a common interface's for the types of development tools.
e.g. it abstract all SCM platform with a standard interface for multiple solutions like Github, Gitlab & Azure Devops.

### Berry

Berry is the AI integration component.
It hooks into Leash to get the data from the development tools.
It pipes the data into the AI model and returns the result back to the user through Leash.

### Terry

Terry is the AI training data extraction component.
It uses leash to extract the data from the development tools.
It transforms the data into training data for the AI model.
It helps to fine tune the AI model with project specific data.
