<!-- Improved compatibility of back to top link: See: https://github.com/othneildrew/Best-README-Template/pull/73 -->
<a name="Cucuota"></a>
<!--
*** Thanks for checking out the Best-README-Template. If you have a suggestion
*** that would make this better, please fork the repo and create a pull request
*** or simply open an issue with the tag "enhancement".
*** Don't forget to give the project a star!
*** Thanks again! Now go create something AMAZING! :D
-->



<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]



<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/kaelthasmanu/cucuota">
    <img src="/cucuota/img/logo.png" alt="Logo" width="80" height="80">
  </a>

<h3 align="center">Cucuota</h3>

  <p align="center">
    Quota system for Squid proxy
    <br />
    <a href="https://github.com/github_username/repo_name"><strong>Explore the docs »</strong></a>
    <br />
    <br />
    <a href="https://github.com/github_username/repo_name">View Demo</a>
    ·
    <a href="https://github.com/kaelthasmanu/cucuota/issues">Report Bug</a>
    ·
    <a href="https://github.com/kaelthasmanu/cucuota/issues">Request Feature</a>
  </p>
</div>



<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

![Example FrontEnd](https://github.com/kaelthasmanu/cucuota/blob/main/cucuota/img/example.jpg)

<p align="right">(<a href="#readme-top">back to top</a>)</p>



### Built With
* [![.Net][.Net]][.Net-url]


<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- GETTING STARTED -->
## Getting Started

This project is an alternative to a quota system for Squid. Client Web for this project [CucuotaWeb](https://github.com/Rooterts/CuCuotaWeb)

### Prerequisites

This is an example of how to list things you need to use the software and how to install them.
* dotnet7 and unzip
  ```sh
  sudo apt install dotnet7 unzip
  ```

### Installation


1. Download files
   ```sh
   wget https://github.com/kaelthasmanu/cucuota/releases/download/0.0.2/linux.zip
   ```
2. Install packages `.Net7 and zip`
   ```sh
   sudo apt install dotnet7 unzip 
   ```
3. Unzip folder `linux-64.zip`
   ```sh
   unzip linux.zip
   ```
4. Create file for quotas `quota.txt`
  ```sh
  touch quota.txt
  ```
5. Create file for banned users `banned.txt`
  ```sh
  touch banned.txt
  ```
7. Crate file with name `appsettings.json`
  ```sh
  touch appsettings.json
  ```
  <p>For its correct functioning, this application needs the appsettings.json file to contain the following information</p>
  ```
  {
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "WorkingFiles": {
    "LogFile": "/Users/manuel/Desktop/cucuota/cucuota/bin/Debug/net7.0/access.log",
    "QuoteFile": "/Users/manuel/Desktop/cucuota/cucuota/bin/Debug/net7.0/quota.txt",
    "BannedFile": "/Users/manuel/Desktop/cucuota/cucuota/bin/Debug/net7.0/banned.txt"
  },
  "LDAPServer": {
    "Server": "domain or ip",
    "ServerPort": 389,
    "UserDN": "manuel",
    "PasswordDN": "password",
    "DN": "ou=Usuarios,dc=domain,dc=cu"
  },
  "URLListen": {
    "base_url": "http://0.0.0.0:5173"
  },
  "JwtOptions": {
    "Issuer": "https://localhost:7004",
    "Audience": "https://localhost:7004",
    "SigningKey": "4a9d45c7b5719c26d673be5944de1994e8432d70aafadf944c0c90b6f9437d1f",
    "ExpirationSeconds": 3600
  }
}
```
6. Change some variables `appsettings.json`
   ```sh
   nano appsettings.json
   ```
7. Run project `cucuota.dll`
   ```sh
   dotnet cucuota.dll
   ```
8. Adapt the systemd configuration for you in file `dotnet`
  ```sh
  nano cucuota
  ```
9. Copy file in the directory `etc/systemd/system`
  ```sh
  cp cucuota /etc/systemd/system/
  ```
10. Reload all services systemd
  ```sh
  sudo systemctl daemon-reload
  ```
11. Enable serivice `cucuota`
  ```sh
   systemctl enable cucuota
   ```
12. Check status service 
  ```sh
  systemctl status cucuota
  ```

<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- USAGE EXAMPLES -->
## Usage

Use this space to show useful examples of how a project can be used. Additional screenshots, code examples and demos work well in this space. You may also link to more resources.

_For more examples, please refer to the [Documentation](https://example.com)_

<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- ROADMAP -->
## Roadmap

- [ ] View Admin
- [ ] Authentication LDAP
- [ ] Dashboard
    - [ ] Behavior Graphs

See the ([open issues](https://github.com/kaelthasmanu/cucuota/issues)) for a full list of proposed features (and known issues).

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE.txt` for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- CONTACT -->
## Contact

Your Name - ([@telegram_handle](https://t.me/king_0f_deathhh)) - manuelalberto.gorrin@gmail.com

Project Link: ([Project Link](https://github.com/kaelthasmanu/cucuota))

<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/kaelthasmanu/cucuota.svg?style=for-the-badge
[contributors-url]: https://github.com/kaelthasmanu/cucuota/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/kaelthasmanu/cucuota.svg?style=for-the-badge
[forks-url]: https://github.com/kaelthasmanu/cucuota/network/members
[stars-shield]: https://img.shields.io/github/stars/kaelthasmanu/cucuota.svg?style=for-the-badge
[stars-url]: https://github.com/kaelthasmanu/cucuota/stargazers
[issues-shield]: https://img.shields.io/github/issues/kaelthasmanu/cucuota.svg?style=for-the-badge
[issues-url]: https://github.com/kaelthasmanu/cucuota/issues
[license-shield]: https://img.shields.io/github/license/kaelthasmanu/cucuota.svg?style=for-the-badge
[license-url]: https://github.com/github_username/repo_name/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/manuel-gorrin-095242238
[product-screenshot]: images/screenshot.png
[.Net]: https://neosmart.net/blog/wp-content/uploads/2019/06/dot-NET-Core-300x300.png
[.Net-url]: https://dotnet.microsoft.com/en-us/apps/aspnet
