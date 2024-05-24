# OnlineLibriary

## Concept Idea
Website that offers ability to publish and read books. This project aims to address the need for a platform where people can find books of any genre.

## Description of features
- The programm was made with an onion architecture.
- The program was made taking into account all the SOLID principles.
- The program was made using different patterns.
- Special features:
    - JWT authentication and authorization.
    - Hashed password with salt.

## Architecture
![Image1](./OnlineLibriary/lmages/photo_2023-06-03_19-55-37.jpg)

## CI/CD
[![.NET](https://github.com/MrSampy/OnlineLibriary/actions/workflows/dotnet.yml/badge.svg)](https://github.com/MrSampy/OnlineLibriary/actions/workflows/dotnet.yml)</br>
Short instruction for docker:
- docker build -t 'image-name' . --no-cache
- docker run -p 5296:80 -t 'image-name'
- Link http://localhost:5296/swagger you will have an ability to test api

## Authors
Kolosov Serhii — [@MrSampy](www.t.me/MrSampy)

## Contributing
If you have any ideas for improving the program, feel free to add new Issues on the [Issues page](https://github.com/MrSampy/OnlineLibriary/issues).

## License
>**Note**: This program is distributed under an MIT License.

## Future
1. Add FrontEnd part.
2. Add more unique features. 
