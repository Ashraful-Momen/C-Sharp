
---

# ✅ C# OOP Notes with Simple Code

---

## ✅ Why Use **Abstract Class**:

* Use when we need a **common base class**, but some methods need to be **changed** in child classes.
* Helps in **partial implementation**.

```csharp
abstract class Animal // Abstract Class
{
    public abstract void Sound(); // Abstract Method (No Body)
    public void Sleep() => Console.WriteLine("Sleeping...");
}

class Dog : Animal // Concrete Class (Inherits Abstract)
{
    public override void Sound() => Console.WriteLine("Dog barks");
}
```

---

## ✅ **Concrete Class**:

* A class that **inherits an abstract class** or implements an interface.

```csharp
// Dog is a concrete class above because it inherits Animal.
```

---

## ✅ **Abstract Class** (Summary):

* Contains **abstract methods** (must be overridden in child classes).
* Can also have **normal methods** with body.
* Child classes must **override abstract methods**.

---

## ✅ **Interface**:

* No variables allowed (best practice).
* Only method declarations.
* Used for flexibility.
* Starts with `I` (naming convention).

```csharp
interface IPayment
{
    void Pay();
}

class PaypalPayment : IPayment
{
    public void Pay() => Console.WriteLine("Paid with PayPal");
}
```

---

## ✅ Context:

* Meaning: A **block of code** or a code segment.
* Example:

```csharp
{
    Console.WriteLine("This is a code block.");
}
```

---

## ✅ **Strategy Design Pattern**:

* Add new features **without changing old code**.
* Often uses Interfaces.

```csharp
interface IPaymentStrategy
{
    void Pay();
}

class CreditCard : IPaymentStrategy
{
    public void Pay() => Console.WriteLine("Paid with Credit Card");
}

class PaymentContext
{
    private IPaymentStrategy paymentStrategy;

    public PaymentContext(IPaymentStrategy strategy)
    {
        paymentStrategy = strategy;
    }

    public void ProcessPayment() => paymentStrategy.Pay();
}
```

---

## ✅ **SOLID Principle** (Summary):

**S** - Single Responsibility
**O** - Open/Closed
**L** - Liskov Substitution
**I** - Interface Segregation
**D** - Dependency Inversion

---

### ✅ **Interface Segregation Principle**:

> Don’t force a class to implement methods it doesn’t need.

```csharp
interface IPrinter
{
    void Print();
}

interface IScanner
{
    void Scan();
}

class SimplePrinter : IPrinter
{
    public void Print() => Console.WriteLine("Printing...");
}
```

---

## ✅ **DRY**: *(Don't Repeat Yourself)*

* Avoid repeating code.

```csharp
// Bad (Repetition)
Console.WriteLine("Hello");
Console.WriteLine("Hello");

// Good (Reuse)
void Greet() => Console.WriteLine("Hello");
Greet();
Greet();
```

---

## ✅ **YAGNI** *(You Aren’t Gonna Need It)*:

> Don’t add features until necessary.

```csharp
// ❌ Bad: Adding unused function
void FutureFeature() { /* unused */ }

// ✅ Good: Add only when needed
```

---

### ✅ ✅ Summary:

| Concept          | Key Idea                                  |
| ---------------- | ----------------------------------------- |
| Abstract Class   | Shared base + must override some methods  |
| Concrete Class   | Class that inherits abstract/interface    |
| Interface        | Only methods, no variables, flexible      |
| Strategy Pattern | Add new feature without changing old code |
| SOLID            | 5 core OOP principles                     |
| Interface Seg.   | No force to implement unused methods      |
| DRY              | Don’t repeat yourself                     |
| YAGNI            | Don’t add code you don’t need             |

---

