// Single ton:
// at a time create 1 object and provide everyone who need the object . 
//example : log writing object , DB connection . 
// alltime create 1 object and provide every class who clss object form single ton class. example : log , db connection etc. 

using System;
using System.Threading;

/// Blog Controller
/// Top 10 Controllers

public class Logger {
    private Logger () {
        Console.WriteLine ("Logger created");
    }

    public static Logger _instance = null;
    public static readonly object _lock = new object();

    public static Logger GetInstance() {
        if(_instance == null) {
            lock(_lock) {// this line block thread : any one thread can go this block => thread1, thread2, thread3 then create any object . after create one object other thread can't create any object(single ton)
                if(_instance == null) { // thread2
                    _instance = new Logger(); // thread1
                }
            }
        }

        return _instance;
    }

    public void Log (string message) {
        Console.WriteLine (message);
    }
}

class Program {
    public static void Main () {
        Thread thread1 = new Thread(() => {
            Logger log1 = Logger.GetInstance();
        });
        Thread thread2 = new Thread(() => {
            Logger log2 = Logger.GetInstance();
        });
        Thread thread3 = new Thread(() => {
            Logger log3 = Logger.GetInstance();
        });

        thread1.Start();
        thread2.Start();
        thread3.Start();

        thread1.Join();
        thread2.Join();
        thread3.Join();
    }
}







// Factory Method with Stretagy Design Pattern: 
// create multiple object for multiople call for creatting object. 
// this is the normal class : if other class need object from factory class : alway provide new object for every one who call for object. 

using System;
using System.Collections.Generic;

public interface ISend {
    public void Send();
}

public interface ILog {
    public void Log();
}

public interface ISave {
    public void Save();
}

public class EmailNotify : ISend, ILog, ISave {
    public string Email { get; set; }

    public void Send() {
        Console.WriteLine("Sending email to " + Email);
    }

    public void Log() {
        Console.WriteLine("Logging email to " + Email);
    }

    public void Save() {
        Console.WriteLine("Saving db to " + Email);
    }
}

public class SMSNotify : ISend, ILog, ISave {
    public string Phone { get; set; }

    public void Send() {
        Console.WriteLine("Sending SMS to " + Phone);
    }

    public void Log() {
        Console.WriteLine("Logging SMS to " + Phone);
    }

    public void Save() {
        Console.WriteLine("Saving db to " + Phone);
    }
}

public class PushNotify : ISend, ILog {
    public string Token { get; set; }

    public void Send() {
        Console.WriteLine("Sending Push to " + Token);
    }

    public void Log() {
        Console.WriteLine("Logging Push to " + Token);
    }
}

public class WhatsappNotify : ISend, ILog {
    public string Phone { get; set; }

    public void Send() {
        Console.WriteLine("Sending Whatsapp to " + Phone);
    }

    public void Log() {
        Console.WriteLine("Logging Whatsapp to " + Phone);
    }
}

public class NotifyContext {
    public ISend send { get; set; }
    public ILog log { get; set; }
    public ISave save { get; set; }

    public NotifyContext(ISend send, ILog log, ISave save) {
        this.send = send;
        this.log = log;
        this.save = save;
    }

    public void Process() {
        send.Send();
        log.Log();

        if(save != null) {
            save.Save();
        }
    }
}

public interface INotificationContextCreator {
    public NotifyContext Create();
}

public class EmailNotificationContextCreator : INotificationContextCreator {
    public NotifyContext Create() {
        return new NotifyContext (
            new EmailNotify { Email = "test@test.com" },
            new EmailNotify { Email = "test@test.com" },
            new EmailNotify { Email = "test@test.com" }
            );
    }
}

public class SMSNotificationContextCreator : INotificationContextCreator {
    public NotifyContext Create() {
        return new NotifyContext (
            new SMSNotify { Phone = "123456789" },
            new SMSNotify { Phone = "123456789" },
            new SMSNotify { Phone = "123456789" }
            );
    }
}

public class PushNotificationContextCreator : INotificationContextCreator {
    public NotifyContext Create() {
        return new NotifyContext (
            new PushNotify { Token = "123456789" },
            new PushNotify { Token = "123456789" },
            null
            );
    }
}

public class WhatsappNotificationContextCreator : INotificationContextCreator {
    public NotifyContext Create() {
        return new NotifyContext (
            new WhatsappNotify { Phone = "123456789" },
            new WhatsappNotify { Phone = "123456789" },
            null
            );
    }
}

class Program {
    public static void Main () {
        INotificationContextCreator emailCreator = new EmailNotificationContextCreator();
        NotifyContext emailContext = emailCreator.Create();

        INotificationContextCreator smsCreator = new SMSNotificationContextCreator();
        NotifyContext smsContext = smsCreator.Create();

        INotificationContextCreator pushCreator = new PushNotificationContextCreator();
        NotifyContext pushContext = pushCreator.Create();

        INotificationContextCreator whatsappCreator = new WhatsappNotificationContextCreator();
        NotifyContext whatsappContext = whatsappCreator.Create();

        emailContext.Process();
        smsContext.Process();
        pushContext.Process();
        whatsappContext.Process();
    }
}






// Factory Method, Signleton with Strategy Design Pattern:


using System;
using System.Collections.Generic;

public interface ILogger {
    public void Log(string message);
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
        Console.WriteLine("This is from log - {0}",message);
    }
}

public interface ISend {
    public void Send();
}

public interface ILog {
    public void Log();
}

public interface ISave {
    public void Save();
}

public class EmailNotify : ISend, ILog, ISave {
    public string Email { get; set; }

    public void Send() {
        Console.WriteLine("Sending email to " + Email);
    }

    public void Log() {
        ILogger logger = Logger.GetInstance();
        logger.Log("Logging email to " + Email);
    }

    public void Save() {
        Console.WriteLine("Saving db to " + Email);
    }
}

public class SMSNotify : ISend, ILog, ISave {
    public string Phone { get; set; }

    public void Send() {
        Console.WriteLine("Sending SMS to " + Phone);
    }

    public void Log() {
        ILogger logger = Logger.GetInstance();
        logger.Log("Logging SMS to " + Phone);
    }

    public void Save() {
        Console.WriteLine("Saving db to " + Phone);
    }
}

public class PushNotify : ISend, ILog {
    public string Token { get; set; }

    public void Send() {
        Console.WriteLine("Sending Push to " + Token);
    }

    public void Log() {
        ILogger logger = Logger.GetInstance();
        logger.Log("Logging Push to " + Token);
    }
}

public class WhatsappNotify : ISend, ILog {
    public string Phone { get; set; }

    public void Send() {
        Console.WriteLine("Sending Whatsapp to " + Phone);
    }

    public void Log() {
        ILogger logger = Logger.GetInstance();
        logger.Log("Logging Whatsapp to " + Phone);
    }
}

public class NotifyContext {
    public ISend send { get; set; }
    public ILog log { get; set; }
    public ISave save { get; set; }

    public NotifyContext(ISend send, ILog log, ISave save) {
        this.send = send;
        this.log = log;
        this.save = save;
    }

    public void Process() {
        send.Send();
        log.Log();

        if(save != null) {
            save.Save();
        }
    }
}

public interface INotificationContextCreator {
    public NotifyContext Create();
}

public class EmailNotificationContextCreator : INotificationContextCreator {
    public NotifyContext Create() {
        return new NotifyContext (
            new EmailNotify { Email = "test@test.com" },
            new EmailNotify { Email = "test@test.com" },
            new EmailNotify { Email = "test@test.com" }
            );
    }
}

public class SMSNotificationContextCreator : INotificationContextCreator {
    public NotifyContext Create() {
        return new NotifyContext (
            new SMSNotify { Phone = "123456789" },
            new SMSNotify { Phone = "123456789" },
            new SMSNotify { Phone = "123456789" }
            );
    }
}

public class PushNotificationContextCreator : INotificationContextCreator {
    public NotifyContext Create() {
        return new NotifyContext (
            new PushNotify { Token = "123456789" },
            new PushNotify { Token = "123456789" },
            null
            );
    }
}

public class WhatsappNotificationContextCreator : INotificationContextCreator {
    public NotifyContext Create() {
        return new NotifyContext (
            new WhatsappNotify { Phone = "123456789" },
            new WhatsappNotify { Phone = "123456789" },
            null
            );
    }
}

class Program {
    public static void Main () {
        INotificationContextCreator emailCreator = new EmailNotificationContextCreator();
        NotifyContext emailContext = emailCreator.Create();

        INotificationContextCreator smsCreator = new SMSNotificationContextCreator();
        NotifyContext smsContext = smsCreator.Create();

        INotificationContextCreator pushCreator = new PushNotificationContextCreator();
        NotifyContext pushContext = pushCreator.Create();

        INotificationContextCreator whatsappCreator = new WhatsappNotificationContextCreator();
        NotifyContext whatsappContext = whatsappCreator.Create();

        emailContext.Process();
        smsContext.Process();
        pushContext.Process();
        whatsappContext.Process();
    }
}


