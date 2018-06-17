<h2 align="center">Please know that this website is still undergoing development and testing !</h2>

## Introduction

QuidditchRefTraining is a website dedicated to train Quidditch referees and players, by offering real life representation and videos.

I started this project as a way to learn C# and ASP.Net, thus I did not knew a lot of good practices and usefull tools at first (like dependency injection)

Technology used : Asp.NET MVC, EntityFramework with Razor

## How to use

- Clone this project and https://github.com/pyrovoice/BDD locally and open this project in visual studio.
- Run the Enable-Migrations command in Package Manager Console, then run Update-Database. This should create the model in your local database.
- Using the files in the BDD project, connect to your local database and run the script to create all needed stored procedures.
- Start the project, connect to the Home controler.

You should reach the homepage. You can know navigate the website, two users user0/password and user1/password are created to be logged in, and a set of mock questions are created to test the Quizz page.

## Features

- Login page
- Homepage leading to the Quizz page. Select Other and Basic to have both radioButtons and checkBoxes answers.
- The profile page, if logged in, will display your previous results. You can also click on a result to check which questions you got wrong.
- The Questions tab, that display all questions currently in base.

## Roadmap & Vision

I discuss the roadmap and vision here : www.reddit.com/r/QuidditchRefTraining
