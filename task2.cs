using System;
using System.Collections.Generic;

// Базовый класс бронирования
public abstract class Reservation
{
    public string ReservationID { get; set; }
    public string CustomerName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public abstract decimal CalculatePrice();
    
    public virtual void DisplayDetails()
    {
        Console.WriteLine($"Бронирование ID: {ReservationID}");
        Console.WriteLine($"Клиент: {CustomerName}");
        Console.WriteLine($"Дата начала: {StartDate:d}");
        Console.WriteLine($"Дата окончания: {EndDate:d}");
        Console.WriteLine($"Стоимость: {CalculatePrice():C}");
    }
}

// Бронирование отеля
public class HotelReservation : Reservation
{
    public string RoomType { get; set; }
    public string MealPlan { get; set; }
    
    public override decimal CalculatePrice()
    {
        int days = (EndDate - StartDate).Days;
        decimal basePrice = days * 50; // Базовая цена
        decimal roomTypeMultiplier = RoomType == "Deluxe" ? 1.5m : 1m;
        decimal mealPlanCost = MealPlan == "All Inclusive" ? days * 30 : days * 15;
        
        return basePrice * roomTypeMultiplier + mealPlanCost;
    }
    
    public override void DisplayDetails()
    {
        base.DisplayDetails();
        Console.WriteLine($"Тип номера: {RoomType}");
        Console.WriteLine($"Тип питания: {MealPlan}");
    }
}

// Бронирование авиабилетов
public class FlightReservation : Reservation
{
    public string DepartureAirport { get; set; }
    public string ArrivalAirport { get; set; }
    
    public override decimal CalculatePrice()
    {
        // Простая логика расчета цены - базовая стоимость + налоги
        decimal basePrice = 200;
        decimal taxes = 50;
        return basePrice + taxes;
    }
    
    public override void DisplayDetails()
    {
        base.DisplayDetails();
        Console.WriteLine($"Аэропорт вылета: {DepartureAirport}");
        Console.WriteLine($"Аэропорт прибытия: {ArrivalAirport}");
    }
}

// Аренда автомобиля
public class CarRentalReservation : Reservation
{
    public string CarType { get; set; }
    public bool InsuranceIncluded { get; set; }
    
    public override decimal CalculatePrice()
    {
        int days = (EndDate - StartDate).Days;
        decimal basePrice = days * 40;
        decimal carTypeMultiplier = CarType == "SUV" ? 1.3m : 1m;
        decimal insuranceCost = InsuranceIncluded ? days * 10 : 0;
        
        return basePrice * carTypeMultiplier + insuranceCost;
    }
    
    public override void DisplayDetails()
    {
        base.DisplayDetails();
        Console.WriteLine($"Тип автомобиля: {CarType}");
        Console.WriteLine($"Страховка включена: {(InsuranceIncluded ? "Да" : "Нет")}");
    }
}

// Система бронирования
public class BookingSystem
{
    private List<Reservation> _reservations = new List<Reservation>();
    private int _nextId = 1;
    
    public Reservation CreateReservation(string reservationType)
    {
        Reservation reservation = reservationType switch
        {
            "Hotel" => new HotelReservation(),
            "Flight" => new FlightReservation(),
            "CarRental" => new CarRentalReservation(),
            _ => throw new ArgumentException("Неизвестный тип бронирования")
        };
        
        reservation.ReservationID = $"RES-{_nextId++}";
        _reservations.Add(reservation);
        return reservation;
    }
    
    public bool CancelReservation(string reservationId)
    {
        var reservation = _reservations.Find(r => r.ReservationID == reservationId);
        if (reservation != null)
        {
            _reservations.Remove(reservation);
            Console.WriteLine($"Бронирование {reservationId} отменено");
            return true;
        }
        return false;
    }
    
    public decimal GetTotalBookingValue()
    {
        decimal total = 0;
        foreach (var reservation in _reservations)
        {
            total += reservation.CalculatePrice();
        }
        return total;
    }
    
    public void DisplayAllReservations()
    {
        Console.WriteLine("\nВсе бронирования:");
        foreach (var reservation in _reservations)
        {
            reservation.DisplayDetails();
            Console.WriteLine();
        }
    }
}

// Пример использования
class Program
{
    static void Main(string[] args)
    {
        var bookingSystem = new BookingSystem();
        
        // Создаем бронирование отеля
        var hotelRes = (HotelReservation)bookingSystem.CreateReservation("Hotel");
        hotelRes.CustomerName = "Иван Иванов";
        hotelRes.StartDate = new DateTime(2023, 6, 1);
        hotelRes.EndDate = new DateTime(2023, 6, 7);
        hotelRes.RoomType = "Deluxe";
        hotelRes.MealPlan = "All Inclusive";
        
        // Создаем бронирование авиабилетов
        var flightRes = (FlightReservation)bookingSystem.CreateReservation("Flight");
        flightRes.CustomerName = "Иван Иванов";
        flightRes.StartDate = new DateTime(2023, 6, 1);
        flightRes.EndDate = new DateTime(2023, 6, 1);
        flightRes.DepartureAirport = "SVO";
        flightRes.ArrivalAirport = "IST";
        
        // Создаем аренду автомобиля
        var carRes = (CarRentalReservation)bookingSystem.CreateReservation("CarRental");
        carRes.CustomerName = "Иван Иванов";
        carRes.StartDate = new DateTime(2023, 6, 1);
        carRes.EndDate = new DateTime(2023, 6, 5);
        carRes.CarType = "SUV";
        carRes.InsuranceIncluded = true;
        
        // Отображаем все бронирования
        bookingSystem.DisplayAllReservations();
        
        // Выводим общую стоимость
        Console.WriteLine($"Общая стоимость всех бронирований: {bookingSystem.GetTotalBookingValue():C}");
        
        // Отменяем одно бронирование
        bookingSystem.CancelReservation(flightRes.ReservationID);
        
        // Снова выводим общую стоимость
        Console.WriteLine($"Общая стоимость после отмены: {bookingSystem.GetTotalBookingValue():C}");
    }
}
