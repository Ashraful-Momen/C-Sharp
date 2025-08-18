using System;
using System.Collections.Generic;

// Singleton Logger (unchanged)
public interface ILogger {
    void Log(string message);
}

public class Logger : ILogger {
    private Logger() { 
        Console.WriteLine("Logger created");
    }

    private static Logger _instance = null;
    private static readonly object _lock = new object();

    public static Logger GetInstance() {
        if (_instance == null) {
            lock (_lock) {
                if (_instance == null) {
                    _instance = new Logger();
                }
            }
        }
        return _instance;
    }

    public void Log(string message) {
        Console.WriteLine("This is from log - {0}", message);
    }
}

// Blueprint Method Pattern Implementation
public abstract class NotificationBlueprint {
    protected ILogger logger;
    
    public NotificationBlueprint() {
        logger = Logger.GetInstance();
    }

    // Template Method - defines the algorithm structure
    public void ProcessNotification() {
        Send();
        Log();
        
        if (SupportsSave()) {
            Save();
        }
    }

    // Abstract methods - must be implemented by concrete classes
    protected abstract void Send();
    protected abstract void Log();
    
    // Hook method - can be overridden if needed
    protected virtual void Save() {
        Console.WriteLine("Saving to database...");
    }
    
    // Hook method - determines if Save should be called
    protected virtual bool SupportsSave() {
        return false;
    }
    
    // Helper method for consistent logging
    protected void LogAction(string action, string target) {
        logger.Log($"{action} to {target}");
    }
}

// Concrete implementations
public class EmailNotification : NotificationBlueprint {
    public string Email { get; set; }
    
    public EmailNotification(string email) {
        Email = email;
    }

    protected override void Send() {
        Console.WriteLine("Sending email to " + Email);
    }

    protected override void Log() {
        LogAction("Logging email", Email);
    }

    protected override void Save() {
        Console.WriteLine("Saving email to database: " + Email);
    }

    protected override bool SupportsSave() {
        return true; // Email notifications support saving
    }
}

public class SMSNotification : NotificationBlueprint {
    public string Phone { get; set; }
    
    public SMSNotification(string phone) {
        Phone = phone;
    }

    protected override void Send() {
        Console.WriteLine("Sending SMS to " + Phone);
    }

    protected override void Log() {
        LogAction("Logging SMS", Phone);
    }

    protected override void Save() {
        Console.WriteLine("Saving SMS to database: " + Phone);
    }

    protected override bool SupportsSave() {
        return true; // SMS notifications support saving
    }
}

public class PushNotification : NotificationBlueprint {
    public string Token { get; set; }
    
    public PushNotification(string token) {
        Token = token;
    }

    protected override void Send() {
        Console.WriteLine("Sending Push notification to " + Token);
    }

    protected override void Log() {
        LogAction("Logging Push notification", Token);
    }

    protected override bool SupportsSave() {
        return false; // Push notifications don't support saving
    }
}

public class WhatsAppNotification : NotificationBlueprint {
    public string Phone { get; set; }
    
    public WhatsAppNotification(string phone) {
        Phone = phone;
    }

    protected override void Send() {
        Console.WriteLine("Sending WhatsApp message to " + Phone);
    }

    protected override void Log() {
        LogAction("Logging WhatsApp message", Phone);
    }

    protected override bool SupportsSave() {
        return false; // WhatsApp notifications don't support saving
    }
}

// Factory Pattern for creating notifications
public abstract class NotificationFactory {
    public abstract NotificationBlueprint CreateNotification();
}

public class EmailNotificationFactory : NotificationFactory {
    private string email;
    
    public EmailNotificationFactory(string email) {
        this.email = email;
    }

    public override NotificationBlueprint CreateNotification() {
        return new EmailNotification(email);
    }
}

public class SMSNotificationFactory : NotificationFactory {
    private string phone;
    
    public SMSNotificationFactory(string phone) {
        this.phone = phone;
    }

    public override NotificationBlueprint CreateNotification() {
        return new SMSNotification(phone);
    }
}

public class PushNotificationFactory : NotificationFactory {
    private string token;
    
    public PushNotificationFactory(string token) {
        this.token = token;
    }

    public override NotificationBlueprint CreateNotification() {
        return new PushNotification(token);
    }
}

public class WhatsAppNotificationFactory : NotificationFactory {
    private string phone;
    
    public WhatsAppNotificationFactory(string phone) {
        this.phone = phone;
    }

    public override NotificationBlueprint CreateNotification() {
        return new WhatsAppNotification(phone);
    }
}

class Program {
    public static void Main() {
        // Create different notification factories
        var factories = new List<NotificationFactory> {
            new EmailNotificationFactory("test@test.com"),
            new SMSNotificationFactory("123456789"),
            new PushNotificationFactory("push_token_123"),
            new WhatsAppNotificationFactory("987654321")
        };

        // Process all notifications using the blueprint method
        foreach (var factory in factories) {
            Console.WriteLine("\n" + new string('-', 40));
            var notification = factory.CreateNotification();
            notification.ProcessNotification();
        }
        
        Console.WriteLine("\n" + new string('=', 40));
        Console.WriteLine("All notifications processed!");
    }
}
