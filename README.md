# nocodedb

The nocodedb goal is to remove, or simplify the Data access layer and replace it with a RAD tool that encompases all ecosystems, servers and types of db.  It's free to all. Free as in free beer.


## How does it do it?
 nocodedb delevers data via an object, with 2 tiers to choose from. A restful web service(webAPI) or a C# library. Both deliver currated data in the form of a nested object. Support for SQL queries is available. All data supports automatic pagniation and crud across all tiers of data delivery.


## System Software Components 
A DLL, selfhost web API and web front end.
The c# dll is the core of the application.
The webAPI Uses this core to deliver content.
The webAPI delivers content to the web and is processed by jQuery.


## Data Objects
The system uses meta data to describe database relations. With this meta object the system processes your data into well formed consumable objects. Easily processed by web and modern languages. The data may span servers, databases, and database types.


## Road Map
Angular integration is in the future (4 only)
Other db types planned, Oracle, posgres, sqlite


## Current Support
- Server Support :Windows and Linux (Under Mono)
- Client Suport: Firefox, Chrome, Internet Explorer 8+


## Deployment
Check the DEPLOY.md file for more details   
- Step 1 -  Download the app
- Step 2 -  Run the app. Windows..just click on the (filename) Linux... mono (filename)
- Step 3 -  A web wite will openin your default browser. Folow the UI to build/save/edit your object.
- Step 4 -  Follow deployment instructions on website under DEPLOY tab.


## Prerequisites
- Linux   -> mono, web browser
- Windows -> web browser


## Built With
- c#        -> Monodevelop                          -> https://www.monodevelop.com/
- HTML      -> geany                                -> https://www.geany.org/
- json      -> NewtonSoft                           -> https://www.newtonsoft.com/json/
- webAPI    -> Microsoft.AspNet.WebAPI.OwinSelfHost -> nuget package managment...
- tables    -> mottie/tablesorter                   -> https://github.com/Mottie/tablesorter/
- browser   -> jQuery 3.2.1                         -> https://jquery.com/
- UI        -> bootstrap4                           -> https://getbootstrap.com/
- Glyphs    -> fontawesome 5                        -> http://fontawesome.io/
- branching -> GIT                                  -> https://git-scm.com/


## Contributing
Please read the COINTRIBUTING.md for details on our code of conduct, and the process for submitting pull requests to us.


## Versioning
We use GIT for versioning. For versions available, see the tags on this repository.


## Authors
Charles Watkins


## License
This project is licensed under the GNU General Public License v3 -- se the LICENSE.md file for details



~EOF
