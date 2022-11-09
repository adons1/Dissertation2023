use Customers

select * from Customers c
join CustomerIdentities ci on ci.CustomerId = c.Id
GO