# BotConnectorApp

BotConnectorApp es una aplicación de consola que se conecta a un bot utilizando el servicio Direct Line de Microsoft Bot Framework. La aplicación permite enviar mensajes al bot y recibir respuestas en tiempo real.

## Requisitos

- .NET 8.0 SDK o superior
- Visual Studio Code o Visual Studio 2022 (opcional)

## Configuración

1. Clona el repositorio:

    ```sh
    git clone <URL_DEL_REPOSITORIO>
    cd BotConnectorApp
    ```

2. Configura los valores en `appsettings.json`:

    ```json
    {
      "Settings": {
        "BotId": "49955793-1e68-468f-a956-254f63e56624",
        "BotTenantId": "3409f6f9-9a3e-4a99-aefe-e5a3751d7fec",
        "BotName": "Agentes del Cambio",
        "BotTokenEndpoint": "https://4bc7989fa420e88c8b572c1b7e6f7f.17.environment.api.powerplatform.com/powervirtualagents/botsbyschema/cr24b_agentesDelCambio/directline/token?api-version=2022-03-01-preview",
        "EndConversationMessage": "quit"
      }
    }
    ```

## Ejecución

Para ejecutar la aplicación, utiliza el siguiente comando:

```sh
dotnet watch run --project BotConnectorApp/BotConnectorApp.csproj
```

La aplicación se expondrá en `http://localhost:5000`.

## Endpoints

### Enviar Mensaje

- **URL:** `http://localhost:5000/api/Bot/sendMessage`
- **Método:** `POST`
- **Content-Type:** `application/json`
- **Cuerpo de la Solicitud:**

    ```json
    {
      "From": "+636931026",
      "Body": "Hola"
    }
    ```

- **Respuesta:**

    ```json
    [
      "Respuesta del bot"
    ]
    ```

## Swagger

Puedes acceder a la interfaz de Swagger UI para probar los endpoints en `http://localhost:5000/swagger`.

## Contribuciones

Las contribuciones son bienvenidas. Por favor, abre un issue o un pull request para discutir cualquier cambio que desees realizar.

## Licencia

Este proyecto está licenciado bajo los términos de la licencia MIT.
