# _Pierre's Treats_

#### By: _**Julien Lenaz**

#### _A website for a PierresTreats owner to manage flavors and treats of bakery items_


## Technologies Used

* _C#_
* _MSTest_
* _Git_
* _Visual Studio Code_
* _ASP.NET Core MVC_
* _MySQL_
* _Razor_

## Description
#### _A website which allows a PierresTreats manager to assign Flavors to Treats and vice versa in their bakery _

## Setup/Installation Requirements
* _Clone this respository to your desktop_
* _Navigate to the top level of the directory_
* _Open in your code editor_
* _Create a file named "appsettings.json" in the PierresTreats directory with the following code, replacing the "YOUR" statements with applicable information:
   ```json
    {
      "ConnectionStrings": {
          "DefaultConnection": "Server=localhost;Port=3306;database=YOUR-DATABASE-NAME;uid=YOUR-USERNAME;pwd=YOUR-MYSQL-PASSWORD;"
      }
    }
    ``` 
* _In the PierresTreats dictionary, restore with $ dotnet restore_
* _Download MySQL and MySQL Workbench if you do not have them already_
* _In MySQL Workbench, go to the Navigator > Administration window and select Data Import/Restore_
* _In Import Options select Import from Self-Contained File_
* _Navigate to julien_lenaz.sql in the PierresTreats directory_
* _Under Default Schema to be Imported To, select the New button_
* _Enter the name of your database and click Ok_
* _Click Start Import_
* _In the terminal, enter $ dotnet run to run the program_

## Known Bugs

* _No known bugs_

## License

_[MIT](https://choosealicense.com/licenses/mit/)_

Copyright (c) _2023_ _Julien Lenaz_
