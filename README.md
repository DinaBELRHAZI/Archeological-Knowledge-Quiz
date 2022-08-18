# Archeological Knowledge Quiz (AKQ)

## Application mobile XAMARIN Android

Cette application mobile est reliée à une API Rest permettant la gestion de questions (CRUD).

L'API a été créé avec ASP.NET Core et une base de données NoSQL MongoDB.

## Utilisaion du projet

### Création de la base de données

J'ai créé la base de données et sa data à partir d'articles sur l'archéologie.

Celle-ci est également utilisée dans le projet QuizGame (https://github.com/DinaBELRHAZI/QuizGame)

Utilisez le fichier quiz_archeo.csv pour créer votre bdd.

![bdd](img/Mongodb_archeo_quiz.PNG)



## Composition du projet

### ApiQuiz

ApiQuiz est une API Rest qui va intervenir sur la base de données dès lors qu'elle sera sollicitée. 

#### Swagger

![swagger](img/swagger.PNG)


### Application Xamarin Android

#### Menu Flyout 

Pour faciliter la navigation dans l'application, un menu flyout est accessible sur toutes les pages.

![menu](img/menu.PNG)

#### Liste des questions

![list](img/list.PNG)

#### Détails d'une question

![details](img/details.PNG)

#### Modifier une question

![update](img/update.PNG)

#### Ajout d'une question

![add](img/add.PNG)

#### Suppression d'une question

![delete](img/delete.PNG)

