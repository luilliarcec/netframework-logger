# Net Framework Logger

[![latest version](https://img.shields.io/nuget/v/Luilliarcec.Logger)](https://www.nuget.org/packages/Luilliarcec.Logger) 
[![downloads](https://img.shields.io/nuget/dt/Luilliarcec.Logger)](https://www.nuget.org/packages/Luilliarcec.Logger)

Helps keep a record of exceptions generated at runtime.

## Installation

You can install the package via nuget:

```bash
dotnet add package Luilliarcec.Logger
```

## Usage

You can use the Logger in each method that is necessary

```csharp
using Luilliarcec.Logger;
// ...

namespace Test
{
    public class Foo
    {
        public void Sumar() 
        {
            try {
                // ...    
            } catch (Exception ex) {
                Log.Error(ex);    
            }  
        }
    }
}
```

Or you can use the Application.ThreadException event to catch all the unhandled errors.

If you want to send an email with the error report, the package uses an asynchronous delivery method to send the error report to the email. So if you want to get a notification when the submission is complete use the SendCompleted event.

But for the sending of emails to be effective, you must configure your App.config with the following values, or pass these values ​​to the send method of the Log class.

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <appSettings>
    <add key="MAIL_FROM_NAME" value="Sender name"/>
    <add key="MAIL_USERNAME" value="Sender email"/>
    <add key="MAIL_PASSWORD" value="Sender password"/>
    <add key="MAIL_TO" value="Receiver email"/>
    <add key="ClientSettingsProvider.ServiceUri" value=""/>
  </appSettings>
   
</configuration>
```

```csharp
using Luilliarcec.Logger;
// ...

namespace YourProject
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // ...
            Log.SendCompleted += Log_SendCompleted;
            Application.ThreadException += Application_ThreadException;
            // ...
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Log.Error(e.Exception);
            Log.Send();

            // or 

            // Log.Send("Sender name", "Sender email", "Sender password", "Receiver email");
        }

        private static void Log_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            // Notify here that the email has been sent
        }
    }
}
```

The exposed methods to keep an error log are:

| Methods | Return | Description |
| -- | -- | -- |
| Error | `bool` | Save the file with error type |
| Warning | `bool` | Save the file with warning type |
| Info | `bool` | Save the file with information type |
| Drop | `bool` | Delete the file |
| Copy | `bool` | Copy the file to a destination path |
| Exists | `bool` | Verify that the file exists in the directory |
| Send | `void` | Send an email with the error report |
| Path | `property (get, set)` | Log path, default project root directory |

Follow these tips and have a happy code. 

### Security

If you discover any security related issues, please email luilliarcec@gmail.com instead of using the issue tracker.

## Credits

- [Luis Andrés Arce C.](https://github.com/luilliarcec)
- [All Contributors](../../contributors)

## License

The MIT License (MIT). Please see [License File](LICENSE.md) for more information.
