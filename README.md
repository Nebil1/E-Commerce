# Gebeya E-Commerce web application

**Premier E-commerce site that offers an exceptional online shopping experience for customers**

**Designed for our ALX profolio project**

![Gebeya-website preview](/home_page.png)


## Tech Stack

**Frontend:** HTML, CSS, Bootstrap, Javascript

**Backend:** C#, ASP.NET

**Database:** SQL Server 


## Pre requisities:

- Microsoft Visual Studio
- Microsoft SQL Server Express
- Microsoft SQL Server Management Studio(SSMS)
- .NET 6

## How to run

**1. Install the following**
 - Microsoft Visual Studio
 - Microsoft SQL Server Express
 - Microsoft SQL Server Management Studio(SSMS)
 
**2. Open 'E-Commerce.sln' file with Visual Studio**

**3. Run the following commands on .NET command-line interface (CLI) one by one to install all the dependencies**
```bash
 dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 6.0.6
```
```bash
 dotnet add package Microsoft.AspNetCore.Identity.UI --version 6.0.6
```
```bash
 dotnet add package Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation --version 6.0.6
```
```bash
 dotnet add package Microsoft.AspNetCore.Mvc.ViewFeatures --version 2.2.0
```
```bash
 dotnet add package Microsoft.EntityFrameworkCore --version 6.0.6
```
```bash
 dotnet add package Microsoft.EntityFrameworkCore.Relational --version 6.0.6
```
```bash
 dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 6.0.6
```
```bash
 dotnet add package Microsoft.EntityFrameworkCore.Tools --version 6.0.6
```
```bash
 dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Contracts --version 5.0.2
```
```bash
 dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design --version 6.0.6
```
```bash
 dotnet add package Stripe.net --version 39.119.0
```
**4. Open SQL Server Management Studio and in the "Connect to Database Engine" window type the following:**
```bash
  Servername: .\SQLEXRPESS
  Authentication: Windows Authentication 
```
**5. Everything is setup now! You can run the by clicking on run button in Visual Studio.**
