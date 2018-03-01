using System.Windows;
using StockScreener.Interfaces;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.IO;
using System.Xml;
using System.Diagnostics;

namespace StockScreener.Model
{
    /// <summary>
    /// Implementation of a user info service
    /// Manages lists of users and logging in/ouot
    /// </summary>
    public class UserInfoService : Notifyable, IUserInfoService
    {
        private string userFilePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Screener\\Users.xml";

        [PreferredConstructorAttribute]
        public UserInfoService()
        {
            //If a user file exists, load it in.
            if (File.Exists(userFilePath))
            {
                LoadUsersFromFile(userFilePath);
            }
        }

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

        private bool _failedUserLoadOrSave;
        public bool FailedUserLoadOrSave
        {
            get { return _failedUserLoadOrSave; }
            private set
            {
                _failedUserLoadOrSave = value;
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
            if (!File.Exists(filePath))
                return false;
            //try to deserialize
            try
            {
                Users.Clear();
                using (XmlReader reader = XmlReader.Create(filePath))
                {
                    while (reader.Read())
                    {
                        // Only detect start elements.
                        if (reader.IsStartElement())
                        {
                            // Get element name and switch on it.
                            switch (reader.Name)
                            {
                                case "Users":
                                    XmlReader inner = reader.ReadSubtree();
                                    while (inner.Read())
                                    {
                                        // Only detect start elements.
                                        if (inner.IsStartElement())
                                        {
                                            switch (inner.Name)
                                            {
                                                case "User":
                                                    var user = new User();
                                                    XmlReader inner2 = inner.ReadSubtree();
                                                    user.ReadXml(inner2);
                                                    Users.Add(user);
                                                    break;
                                            }
                                        }
                                    }

                                    break;
                            }
                        }
                    }
                }
                FailedUserLoadOrSave = false;
                return true;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.InnerException);
                FailedUserLoadOrSave = true;
            }
            return false;
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
                    //set the logged in user property to a reference of the found user
                    LoggedInUser = userInList;
                    return true;
                }
            }
            //If we reached here then we didn't find one, return false
            return false;
        }

        /// <summary>
        /// Save users to disk
        /// </summary>
        /// <param name="filePath">If empty will use default path</param>
        /// <returns></returns>
        public bool SaveUsersToFile(string filePath = "")
        {
            try
            {
                if (filePath == "")
                    filePath = userFilePath;
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                using (var stream = File.OpenWrite(filePath))
                {
                    var xmlWriterSettings = new XmlWriterSettings() { Indent = true, NewLineOnAttributes = true };
                    using (var writer = XmlWriter.Create(stream, xmlWriterSettings))
                    {
                        writer.WriteStartElement("Users");
                        foreach (var user in Users)
                        {
                            user.WriteXml(writer);
                        }
                        writer.WriteEndElement();
                    }
                }
                FailedUserLoadOrSave = false;
                return true;
            }
            //On exception write out to the trace and return false;
            catch (Exception e)
            {
                Trace.WriteLine(e.InnerException);
                FailedUserLoadOrSave = true;
            }

            return false;
        }

        public void LogOffUser()
        {
            LoggedInUser = null;
        }

        public bool CreateUser(string user)
        {
            //check for empty
            if (user == "") return false;

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
            SaveUsersToFile(userFilePath);
            LogInUser(newUser.Name);
            return true;
        }
    }
}