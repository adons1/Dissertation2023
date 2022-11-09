using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomersService.TransportTypes.TransportModels;

public class Customer
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime Birthday { get; set; }
    public double Account { get; set; }

    public Customer(Guid id, string firstName, string lastName, string email, DateTime birthday, double account)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Birthday = birthday;
        Account = account;
    }
}
