namespace SellerActiveChallenge.Models
{
    public class Post
    {
        public int userId { get; set; } // FK - User
        public int id { get; set; } // PK
        public string title { get; set; }
        public string body { get; set; }
    };

    public class Comment
    {
        public int postId { get; set; } // FK - Post
        public int id { get; set; } // PK
        public string name { get; set; }
        public string email { get; set; } // AFK - User
        public string body { get; set; }
    }

    public class User
    {
        public int id { get; set; } // PK
        public string name { get; set; }
        public string userName { get; set; }
        public string email { get; set; } // AK - unique among all Users
        public Address address;
        public string phone { get; set; }
        public string website { get; set; }
        public Company company;
    };

        public class Company
    {
        public string name { get; set; }
        public string catchPhrase { get; set; }
        public string bs { get; set; }

    }

    public class Address
    {
        public string street { get; set; }
        public string suite { get; set; }
        public string city { get; set; }
        public string zipcode { get; set; }
        public Geo geo;
    }

    public class Geo
    {
        public string lat { get; set; }
        public string lng { get; set; }
    }

}
