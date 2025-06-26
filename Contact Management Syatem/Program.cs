using System.Text.Json;
using System.Drawing;
using Pastel;
class Contact
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public Contact(string Name, string Email, string Phone)
    {
        this.Name = Name;
        this.Email = Email;
        this.Phone = Phone;
    }
    public void DisplayInformation()
    {
        Console.WriteLine($"Name: {Name.Pastel("#FF5733")}");
        Console.WriteLine($"Email: {Email.Pastel("#33FF57")}");
        Console.WriteLine($"Phone: {Phone.Pastel("#3357FF")}");
    }
    private void UpdateName(string newName)
    {
        Name = newName;
    }
    private void UpdateEmail(string newEmail)
    {
        Email = newEmail;
    }
    private void UpdatePhone(string newPhone)
    {
        Phone = newPhone;
    }
    public void UpdateContact(string type)
    {
        Console.WriteLine($"Enter new {type}:".Pastel(Color.FromArgb(20, 20, 240)));
        string newValue = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(newValue))
        {
            Console.WriteLine("Invalid input. Update aborted.".Pastel(Color.FromArgb(240, 20, 20)));
            return;
        }
        switch (type.ToLower())
        {
            case "name":
                Console.WriteLine($"{Name} updated to {newValue}".Pastel(Color.FromArgb(20, 240, 20)));
                UpdateName(newValue);
                break;
            case "email":
                UpdateEmail(newValue);
                break;
            case "phone":
                UpdatePhone(newValue);
                break;
            default:
                Console.WriteLine("Invalid type specified.");
                break;
        }
    }
    public string getEmail()
    {
        return Email;
    }
    public string getName()
    {
        return Name;
    }
    public string getPhone()
    {
        return Phone;
    }
}
class ContactManagementSystem
{
    private List<Contact> contacts;
    private string LoadContacts()
    {
        try
        {
            return File.ReadAllText("contacts.json");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading contacts: {ex.Message}".Pastel(Color.FromArgb(240, 20, 20)));
            return string.Empty;
        }
    }
    public ContactManagementSystem()
    {
        string json;
        if (File.Exists("contacts.json"))
        {
            json = LoadContacts();
        }
        else
        {
            contacts = new List<Contact>();
            SaveContacts();
            return;
        }

        contacts = string.IsNullOrEmpty(json) ? new List<Contact>() : JsonSerializer.Deserialize<List<Contact>>(json) ?? new List<Contact>();
        contacts.RemoveAll(contact =>
            contact == null ||
            string.IsNullOrWhiteSpace(contact.Name) ||
            string.IsNullOrWhiteSpace(contact.Email) ||
            string.IsNullOrWhiteSpace(contact.Phone)
        );
        SaveContacts();
    }
    private void SaveContacts()
    {
        string json = JsonSerializer.Serialize(contacts, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText("contacts.json", json);
    }
    public void AddContact()
    {
        Console.WriteLine("Enter contact Name:".Pastel(Color.FromArgb(20, 20, 240)));
        string Name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(Name))
        {
            Console.WriteLine("Name cannot be empty.".Pastel(Color.FromArgb(240, 20, 20)));
            return;
        }
        Console.WriteLine("Enter contact Email:".Pastel(Color.FromArgb(20, 20, 240)));
        string Email = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(Email) || !Email.Contains("@"))
        {
            Console.WriteLine("Invalid Email format.".Pastel(Color.FromArgb(240, 20, 20)));
            return;
        }
        Console.WriteLine("Enter contact Phone number:".Pastel(Color.FromArgb(20, 20, 240)));
        string Phone = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(Phone) || Phone.Length < 10 || !Phone.All(char.IsDigit))
        {
            Console.WriteLine("Invalid Phone number. It must be at least 10 digits long and contain only numbers.".Pastel(Color.FromArgb(240, 20, 20)));
            return;
        }
        Contact contact = new Contact(Name, Email, Phone);

        if (contacts.Any(c => c.getEmail() == contact.getEmail()))
        {
            Console.WriteLine("Contact with this Email already exists.".Pastel(Color.FromArgb(240, 20, 20)));
            return;
        }
        contacts.Add(contact);
        Console.WriteLine("Contact added successfully!".Pastel(Color.FromArgb(20, 240, 20)));
        SaveContacts();
    }
    public void DisplayContacts()
    {
        if (contacts.Count == 0)
        {
            Console.WriteLine("No contacts available.".Pastel(Color.FromArgb(240, 20, 20)));
            return;
        }
        foreach (var contact in contacts)
        {
            contact.DisplayInformation();
            Console.WriteLine("-------------------------------");
        }
    }
    private List<Contact> GetContacts(string searchTerm)
    {
        return contacts.Where(c => c.getName().Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || c.getEmail().Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || c.getPhone().Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
    }
    public void SearchContact()
    {
        Console.WriteLine("Enter term search:".Pastel(Color.FromArgb(20, 20, 240)));
        string searchTerm = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            Console.WriteLine("Search term cannot be empty.".Pastel(Color.FromArgb(240, 20, 20)));
            return;
        }
        var foundContacts = GetContacts(searchTerm);
        if (foundContacts.Count == 0)
        {
            Console.WriteLine("No contacts found.".Pastel(Color.FromArgb(240, 20, 20)));
            return;
        }
        foreach (var contact in foundContacts)
        {
            contact.DisplayInformation();
            Console.WriteLine("-------------------------------");
        }
        SaveContacts();
    }
    public void UpdateContact()
    {
        Console.WriteLine("Enter term to search for contact:".Pastel(Color.FromArgb(20, 20, 240)));
        string searchTerm = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            Console.WriteLine("Search term cannot be empty.".Pastel(Color.FromArgb(240, 20, 20)));
            return;
        }
        List<Contact> foundContacts = GetContacts(searchTerm);
        if (foundContacts == null || foundContacts.Count == 0)
        {
            Console.WriteLine("No contacts found.".Pastel(Color.FromArgb(240, 20, 20)));
            return;
        }
        for (int i = 0; i < foundContacts.Count; i++)
        {
            Console.WriteLine($"Contact ID {i + 1}:");
            foundContacts[i].DisplayInformation();
            Console.WriteLine("-------------------------------");
        }
        Console.WriteLine("Enter the ID of the contact you want to update:".Pastel(Color.FromArgb(20, 20, 240)));
        if (!int.TryParse(Console.ReadLine(), out int contactNumber) || contactNumber < 1 || contactNumber > foundContacts.Count)
        {
            Console.WriteLine("Invalid contact number.".Pastel(Color.FromArgb(240, 20, 20)));
            return;
        }
        Contact contactToUpdate = foundContacts[contactNumber - 1];
        Console.WriteLine("What do you want to update? (Name/Email/Phone)".Pastel(Color.FromArgb(20, 20, 240)));
        string updateType = Console.ReadLine()?.ToLower();
        if (string.IsNullOrWhiteSpace(updateType))
        {
            Console.WriteLine("Invalid update type.".Pastel(Color.FromArgb(240, 20, 20)));
            return;
        }
        switch (updateType)
        {
            case "name":
                contactToUpdate.UpdateContact("Name");
                break;
            case "email":
                contactToUpdate.UpdateContact("Email");
                break;
            case "phone":
                contactToUpdate.UpdateContact("Phone");
                break;
            default:
                Console.WriteLine("Invalid update type.".Pastel(Color.FromArgb(240, 20, 20)));
                return;
        }
        SaveContacts();
        Console.WriteLine("Contact updated successfully!".Pastel(Color.FromArgb(20, 240, 20)));
    }
    public void DeleteContact()
    {
        Console.WriteLine("Enter term to search for contact:".Pastel(Color.FromArgb(20, 20, 240)));
        string searchTerm = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            Console.WriteLine("Search term cannot be empty.".Pastel(Color.FromArgb(240, 20, 20)));
            return;
        }
        List<Contact> foundContacts = GetContacts(searchTerm);
        if (foundContacts == null || foundContacts.Count == 0)
        {
            Console.WriteLine("No contacts found.".Pastel(Color.FromArgb(240, 20, 20)));
            return;
        }
        for (int i = 0; i < foundContacts.Count; i++)
        {
            Console.WriteLine($"Contact ID {i + 1}:");
            foundContacts[i].DisplayInformation();
            Console.WriteLine("-------------------------------");
        }
        Console.WriteLine("Enter the ID of the contact you want to delete:".Pastel(Color.FromArgb(20, 20, 240)));
        if (!int.TryParse(Console.ReadLine(), out int contactNumber) || contactNumber < 1 || contactNumber > foundContacts.Count)
        {
            Console.WriteLine("Invalid contact number.".Pastel(Color.FromArgb(240, 20, 20)));
            return;
        }
        contacts.Remove(foundContacts[contactNumber - 1]);
        SaveContacts();
        Console.WriteLine("Contact deleted successfully!".Pastel(Color.FromArgb(20, 240, 20)));
    }
}
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Contact Management System!".Pastel(Color.FromArgb(20, 240, 240)));
        ContactManagementSystem cms = new ContactManagementSystem();
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\nChoose an option:");
            Console.WriteLine("1. Add Contact".Pastel(Color.FromArgb(20, 240, 20)));
            Console.WriteLine("2. Display Contacts".Pastel(Color.FromArgb(20, 240, 20)));
            Console.WriteLine("3. Search Contact".Pastel(Color.FromArgb(20, 240, 20)));
            Console.WriteLine("4. Update Contact".Pastel(Color.FromArgb(20, 240, 20)));
            Console.WriteLine("5. Delete Contact".Pastel(Color.FromArgb(20, 240, 20)));
            Console.WriteLine("6. Exit".Pastel(Color.FromArgb(240, 20, 20)));

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    cms.AddContact();
                    break;
                case "2":
                    cms.DisplayContacts();
                    break;
                case "3":
                    cms.SearchContact();
                    break;
                case "4":
                    cms.UpdateContact();
                    break;
                case "5":
                    cms.DeleteContact();
                    break;
                case "6":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.".Pastel(Color.FromArgb(240, 20, 20)));
                    break;
            }
        }
    }
}