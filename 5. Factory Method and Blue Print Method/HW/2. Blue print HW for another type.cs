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

// =================================================================
// STRATEGY PATTERN - Different strategies for each operation
// =================================================================

// Strategy interfaces
public interface ISendStrategy {
    void Send(string target, string content);
}

public interface ILogStrategy {
    void Log(string target, string action);
}

public interface ISaveStrategy {
    void Save(string target, string data);
    bool IsSupported { get; }
}

// =================================================================
// CONCRETE STRATEGIES - Email
// =================================================================
public class EmailSendStrategy : ISendStrategy {
    public void Send(string target, string content) {
        Console.WriteLine($"📧 Sending email to {target}: {content}");
    }
}

public class EmailLogStrategy : ILogStrategy {
    private ILogger logger = Logger.GetInstance();
    
    public void Log(string target, string action) {
        logger.Log($"Email {action} for {target}");
    }
}

public class EmailSaveStrategy : ISaveStrategy {
    public bool IsSupported => true;
    
    public void Save(string target, string data) {
        Console.WriteLine($"💾 Saving email to database: {target} - {data}");
    }
}

// =================================================================
// CONCRETE STRATEGIES - SMS
// =================================================================
public class SMSSendStrategy : ISendStrategy {
    public void Send(string target, string content) {
        Console.WriteLine($"📱 Sending SMS to {target}: {content}");
    }
}

public class SMSLogStrategy : ILogStrategy {
    private ILogger logger = Logger.GetInstance();
    
    public void Log(string target, string action) {
        logger.Log($"SMS {action} for {target}");
    }
}

public class SMSSaveStrategy : ISaveStrategy {
    public bool IsSupported => true;
    
    public void Save(string target, string data) {
        Console.WriteLine($"💾 Saving SMS to database: {target} - {data}");
    }
}

// =================================================================
// CONCRETE STRATEGIES - Push Notification
// =================================================================
public class PushSendStrategy : ISendStrategy {
    public void Send(string target, string content) {
        Console.WriteLine($"🔔 Sending Push notification to {target}: {content}");
    }
}

public class PushLogStrategy : ILogStrategy {
    private ILogger logger = Logger.GetInstance();
    
    public void Log(string target, string action) {
        logger.Log($"Push notification {action} for {target}");
    }
}

public class PushSaveStrategy : ISaveStrategy {
    public bool IsSupported => false; // Push notifications don't save
    
    public void Save(string target, string data) {
        // Not implemented for push notifications
    }
}

// =================================================================
// CONCRETE STRATEGIES - WhatsApp
// =================================================================
public class WhatsAppSendStrategy : ISendStrategy {
    public void Send(string target, string content) {
        Console.WriteLine($"💬 Sending WhatsApp to {target}: {content}");
    }
}

public class WhatsAppLogStrategy : ILogStrategy {
    private ILogger logger = Logger.GetInstance();
    
    public void Log(string target, string action) {
        logger.Log($"WhatsApp {action} for {target}");
    }
}

public class WhatsAppSaveStrategy : ISaveStrategy {
    public bool IsSupported => false; // WhatsApp notifications don't save
    
    public void Save(string target, string data) {
        // Not implemented for WhatsApp notifications
    }
}

// =================================================================
// BLUEPRINT METHOD PATTERN - Template with Strategy injection
// =================================================================
public abstract class NotificationBlueprint {
    // Strategy objects (injected)
    protected ISendStrategy sendStrategy;
    protected ILogStrategy logStrategy;
    protected ISaveStrategy saveStrategy;
    
    // Template data
    protected string target;
    protected string content;
    
    public NotificationBlueprint(ISendStrategy sendStrategy, 
                               ILogStrategy logStrategy, 
                               ISaveStrategy saveStrategy) {
        this.sendStrategy = sendStrategy;
        this.logStrategy = logStrategy;
        this.saveStrategy = saveStrategy;
    }

    // TEMPLATE METHOD - defines the algorithm structure (Blueprint)
    public void ProcessNotification() {
        Console.WriteLine($"\n🚀 Processing {GetNotificationType()} notification...");
        Console.WriteLine("─".PadLeft(50, '─'));
        
        // Step 1: Validate (hook method)
        if (!ValidateInput()) {
            Console.WriteLine("❌ Validation failed!");
            return;
        }
        
        // Step 2: Prepare content (abstract method)
        PrepareContent();
        
        // Step 3: Send using strategy
        ExecuteSend();
        
        // Step 4: Log using strategy
        ExecuteLog();
        
        // Step 5: Save using strategy (conditional)
        ExecuteSave();
        
        // Step 6: Post-process (hook method)
        PostProcess();
        
        Console.WriteLine("✅ Processing completed!");
    }

    // Abstract methods - must be implemented by concrete classes
    protected abstract string GetNotificationType();
    protected abstract void PrepareContent();
    
    // Hook methods - optional override
    protected virtual bool ValidateInput() {
        return !string.IsNullOrEmpty(target);
    }
    
    protected virtual void PostProcess() {
        Console.WriteLine($"📊 Notification metrics updated for {GetNotificationType()}");
    }

    // Strategy execution methods
    private void ExecuteSend() {
        Console.WriteLine("📤 Executing send strategy...");
        sendStrategy.Send(target, content);
    }
    
    private void ExecuteLog() {
        Console.WriteLine("📝 Executing log strategy...");
        logStrategy.Log(target, "processed");
    }
    
    private void ExecuteSave() {
        if (saveStrategy.IsSupported) {
            Console.WriteLine("💾 Executing save strategy...");
            saveStrategy.Save(target, content);
        } else {
            Console.WriteLine("⚠️ Save strategy not supported for this notification type");
        }
    }
    
    // Setter methods for dynamic strategy changes
    public void SetSendStrategy(ISendStrategy strategy) => sendStrategy = strategy;
    public void SetLogStrategy(ILogStrategy strategy) => logStrategy = strategy;
    public void SetSaveStrategy(ISaveStrategy strategy) => saveStrategy = strategy;
}

// =================================================================
// CONCRETE BLUEPRINT IMPLEMENTATIONS
// =================================================================
public class EmailNotification : NotificationBlueprint {
    public EmailNotification(string email, ISendStrategy sendStrategy, 
                           ILogStrategy logStrategy, ISaveStrategy saveStrategy) 
        : base(sendStrategy, logStrategy, saveStrategy) {
        target = email;
    }

    protected override string GetNotificationType() => "Email";
    
    protected override void PrepareContent() {
        content = "Welcome to our service! Thanks for subscribing.";
        Console.WriteLine($"📝 Email content prepared for {target}");
    }
    
    protected override bool ValidateInput() {
        bool isValid = base.ValidateInput() && target.Contains("@");
        if (!isValid) Console.WriteLine("❌ Invalid email format");
        return isValid;
    }
}

public class SMSNotification : NotificationBlueprint {
    public SMSNotification(string phone, ISendStrategy sendStrategy, 
                         ILogStrategy logStrategy, ISaveStrategy saveStrategy) 
        : base(sendStrategy, logStrategy, saveStrategy) {
        target = phone;
    }

    protected override string GetNotificationType() => "SMS";
    
    protected override void PrepareContent() {
        content = "Your verification code is: 123456";
        Console.WriteLine($"📝 SMS content prepared for {target}");
    }
    
    protected override bool ValidateInput() {
        bool isValid = base.ValidateInput() && target.Length >= 10;
        if (!isValid) Console.WriteLine("❌ Invalid phone number");
        return isValid;
    }
}

public class PushNotification : NotificationBlueprint {
    public PushNotification(string token, ISendStrategy sendStrategy, 
                          ILogStrategy logStrategy, ISaveStrategy saveStrategy) 
        : base(sendStrategy, logStrategy, saveStrategy) {
        target = token;
    }

    protected override string GetNotificationType() => "Push";
    
    protected override void PrepareContent() {
        content = "You have a new message!";
        Console.WriteLine($"📝 Push notification content prepared for {target}");
    }
}

public class WhatsAppNotification : NotificationBlueprint {
    public WhatsAppNotification(string phone, ISendStrategy sendStrategy, 
                              ILogStrategy logStrategy, ISaveStrategy saveStrategy) 
        : base(sendStrategy, logStrategy, saveStrategy) {
        target = phone;
    }

    protected override string GetNotificationType() => "WhatsApp";
    
    protected override void PrepareContent() {
        content = "Hello! This is a WhatsApp business message.";
        Console.WriteLine($"📝 WhatsApp content prepared for {target}");
    }
}

// =================================================================
// STRATEGY FACTORY - Creates strategy combinations
// =================================================================
public class StrategyFactory {
    public static (ISendStrategy, ILogStrategy, ISaveStrategy) CreateEmailStrategies() {
        return (new EmailSendStrategy(), new EmailLogStrategy(), new EmailSaveStrategy());
    }
    
    public static (ISendStrategy, ILogStrategy, ISaveStrategy) CreateSMSStrategies() {
        return (new SMSSendStrategy(), new SMSLogStrategy(), new SMSSaveStrategy());
    }
    
    public static (ISendStrategy, ILogStrategy, ISaveStrategy) CreatePushStrategies() {
        return (new PushSendStrategy(), new PushLogStrategy(), new PushSaveStrategy());
    }
    
    public static (ISendStrategy, ILogStrategy, ISaveStrategy) CreateWhatsAppStrategies() {
        return (new WhatsAppSendStrategy(), new WhatsAppLogStrategy(), new WhatsAppSaveStrategy());
    }
}

// =================================================================
// MAIN PROGRAM - Demonstration
// =================================================================
class Program {
    public static void Main() {
        Console.WriteLine("🔥 BLUEPRINT + STRATEGY PATTERN DEMONSTRATION 🔥");
        Console.WriteLine("=".PadLeft(60, '='));

        // Create notifications with their respective strategies
        var notifications = new List<NotificationBlueprint>();

        // Email notification with email strategies
        var (emailSend, emailLog, emailSave) = StrategyFactory.CreateEmailStrategies();
        notifications.Add(new EmailNotification("john@example.com", emailSend, emailLog, emailSave));

        // SMS notification with SMS strategies
        var (smsSend, smsLog, smsSave) = StrategyFactory.CreateSMSStrategies();
        notifications.Add(new SMSNotification("1234567890", smsSend, smsLog, smsSave));

        // Push notification with push strategies
        var (pushSend, pushLog, pushSave) = StrategyFactory.CreatePushStrategies();
        notifications.Add(new PushNotification("token_abc123", pushSend, pushLog, pushSave));

        // WhatsApp notification with WhatsApp strategies
        var (whatsappSend, whatsappLog, whatsappSave) = StrategyFactory.CreateWhatsAppStrategies();
        notifications.Add(new WhatsAppNotification("0987654321", whatsappSend, whatsappLog, whatsappSave));

        // Process all notifications using the blueprint method
        foreach (var notification in notifications) {
            notification.ProcessNotification();
        }

        Console.WriteLine("\n" + "=".PadLeft(60, '='));
        Console.WriteLine("🎉 DEMONSTRATION OF STRATEGY FLEXIBILITY 🎉");
        Console.WriteLine("=".PadLeft(60, '='));

        // Demonstrate strategy flexibility - SMS notification with email strategies
        Console.WriteLine("\n🔄 Changing strategies dynamically...");
        var hybridNotification = new SMSNotification("5555555555", emailSend, emailLog, emailSave);
        Console.WriteLine("📱➡️📧 SMS notification using EMAIL strategies:");
        hybridNotification.ProcessNotification();

        Console.WriteLine("\n🏁 All demonstrations completed!");
    }
}
