using System.Windows;
using StockScreener.Interfaces;
using System.Collections.Generic;
namespace StockScreener.Model
{
    /// <summary>
    /// Implementation of a user info service
    /// Manages lists of users and logging in/ouot
    /// </summary>
    public class UserInfoService : Notifyable, IUserInfoService
    {

        private IUser _loggedInUser;
        /// <summary>
        /// Reference to the logged in user
        /// </summary>
        public IUser LoggedInUser
        {
            get { return _loggedInUser; }
            private set
            {
                _loggedInUser = value;
                RaisePropertyChanged();
            }
        }

        private List<IUser> _users = new List<IUser>();
        /// <summary>
        /// All available users
        /// </summary>
        public List<IUser> Users
        {
            get { return _users; }
            private set
            {
                _users = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Populate the list of users from a file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public bool LoadUsersFromFile(string filePath)
        {
            //todo
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Log a user in if they exist in the list of users
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool LogInUser(string userName)
        {
            //iterate through each list in _users and check for userName match

            foreach (var userInList in _users)
            {
                if (userName.ToLower() == userInList.Name.ToLower())
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("Incorrect username! Please try again!");
                    return false;
                }
            }
            //END TESTING




            ////eventually should do below to only login if we have it

            ////Do we have a user that matches the username
            ////Make lowercase to ignore casing
            //if(Users.Any(x=>x.Name.ToLower() == userName.ToLower()))
            //{
            //    //Set the logged in user = to the first item in the list with the same name
            //    LoggedInUser = Users.FirstOrDefault(x => x.Name.ToLower() == userName.ToLower());
            //    return true;
            //}
            //return false;
        }


        public bool SaveUsersToFile(string filePath)
        {
            //TODO
            throw new System.NotImplementedException();
        }

        public void LogOffUser()
        {
            LoggedInUser = null;
        }

        public bool CreateUser(string user)
        {
            var newUser = new User(user);
            foreach (var knownUser in _users)
            {
                if (newUser.Name.ToLower() == knownUser.Name.ToLower())
                {
                    //User already exists in the list, return false
                    return false;
                }
            }
            //If we got here user does not exist in the list, add it to the users and log them in
            Users.Add(newUser);
            LogInUser(newUser.Name);
            return true;
        }
    }
}