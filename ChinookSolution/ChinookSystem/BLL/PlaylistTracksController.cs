using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using ChinookSystem.Entities;
using ChinookSystem.ViewModels;
using ChinookSystem.DAL;
using System.ComponentModel;
using FreeCode.Exceptions;
#endregion

namespace ChinookSystem.BLL
{

    public class PlaylistTracksController
    {
        //class level variable that will hold multiple strings, each representing an error message. 

        List<Exception> brokenRules = new List<Exception>();

        public List<UserPlaylistTrack> List_TracksForPlaylist(
            string playlistname, string username)
        {
            using (var context = new ChinookSystemContext())
            {

                var results = context.PlaylistTracks.Where(x => x.Playlist.Name.Equals(playlistname) && x.Playlist.UserName.Equals(username)).OrderBy(x => x.TrackNumber).Select(x => new UserPlaylistTrack
                {
                    TrackID = x.TrackId,
                    TrackNumber = x.TrackNumber,
                    TrackName = x.Track.Name,
                    Milliseconds = x.Track.Milliseconds,
                    UnitPrice = x.Track.UnitPrice
                }).ToList();

                return results;
            }
        }//eom

        public void Add_TrackToPLaylist(string playlistname, string username, int trackid,
                                        string song)
        {
            Playlist playlistExist = null;
            PlaylistTrack playlisttrackExist = null;
            int tracknumber = 0;
            using (var context = new ChinookSystemContext())
            {
                //this class is in what is called the Business Logic Layer
                //Business Logic is the rules of your business
                //Business Logic ensures that rules and data are what is expected
                //Rules:
                //  rule: a track can only exist once on a playlist
                //  rule: playlist names can only be used once for a user, different users
                //        many have the same playlist name
                //  rule: each track on a playlist is assigned a continious
                //        track number
                //
                //The BLL method should also ensure that data exists for
                //      the processing of the transaction
                if (string.IsNullOrEmpty(playlistname))
                {
                    //there is a data error
                    //setting up an error message
                    brokenRules.Add(new BusinessRuleException<string>("Playlist name is missing. Unable to add track",
                                "Playlist Name", playlistname));
                }
                if (string.IsNullOrEmpty(username))
                {
                    //there is a data error
                    //setting up an error message
                    brokenRules.Add(new BusinessRuleException<string>("User name is missing. Unable to add track",
                                "User Name", username));
                }

                //does the playlist already exist
                playlistExist = (from x in context.Playlists
                                 where x.Name.Equals(playlistname) &&
                                         x.UserName.Equals(username)
                                 select x).FirstOrDefault();
                if (playlistExist == null)
                {
                    //new playlist
                    //tasks:
                    //  create a new instance of the playlist class
                    //  load the instance with data
                    //  stage the add of the new instance
                    //  set the tracknumber to 1
                    playlistExist = new Playlist()
                    {
                        Name = playlistname,
                        UserName = username
                    };
                    context.Playlists.Add(playlistExist); //parent is STAGED, in memory
                    tracknumber = 1;
                }
                else
                {
                    //existing playlist
                    //tasks:
                    //  does the track already exist on the playlist? if so, error
                    //  if not, find the highest current tracknumber, increment by 1 (rule)
                    playlisttrackExist = (from x in context.PlaylistTracks
                                          where x.Playlist.Name.Equals(playlistname) &&
                                                  x.Playlist.UserName.Equals(username) &&
                                                  x.TrackId == trackid
                                          select x).FirstOrDefault();
                    if (playlisttrackExist == null)
                    {
                        //track does not exist on the desired playlist
                        tracknumber = (from x in context.PlaylistTracks
                                       where x.Playlist.Name.Equals(playlistname) &&
                                                   x.Playlist.UserName.Equals(username)
                                       select x.TrackNumber).Count();
                        tracknumber++;
                    }
                    else
                    {
                        //business rule broken. track DOES exist already on the desired
                        //  playlist
                        brokenRules.Add(new BusinessRuleException<string>("Track already on playlist",
                               nameof(song), song));
                    }
                }
                //add the track to the playlist in PlaylistTracks
                //create an instance
                playlisttrackExist = new PlaylistTrack();
                //load the instance
                playlisttrackExist.TrackId = trackid;
                playlisttrackExist.TrackNumber = tracknumber;

                //add the instance
                //?????????????
                //What is the playlist id
                //if the playlist exists then we know the id
                //BUT if the playlist is NEW, we DO NOT know the id

                //in one case the id is known BUT in the second case
                //  where the new record is ONLY STAGED, NO primary key
                //  value has been generated yet. <-- problem
                //if you access the new playlist record the pkey would be 0
                //  (default numeric value)

                //the solution to BOTH of these scenarios is to use
                //      navigational properties during the ACTUAL .Add commong
                //      for the new playlisttrack record
                //the entityframework will, on your behave, ensure that the
                //      adding of records to the database will be done in te
                //      appropriate order AND will add the missing compound pkey
                //      value (PlaylistId) to the new playlisttrack record

                //NOT LIKE THIS !!!!!!!! THIS IS WRONG!!!!!!!
                //context.PlaylistTracks.Add(playlisttrackExist);

                //INSTEAD, do the STAGING using the parent.navproperty.Add(xxxx);
                playlistExist.PlaylistTracks.Add(playlisttrackExist);

                //do the commit
                //check to see if ANY business rule exceptions occured
                //
                if (brokenRules.Count() > 0)
                {
                    //at least one error was recorded during the processing of the
                    //  transaction
                    throw new BusinessRuleCollectionException("Add Playlist Track Concerns:", brokenRules);
                }
                else
                {
                    //COMMIT THE TRANSACTION
                    //ALL the staged records will be sent to sql for processing
                    //the transaction is complete
                    //NOTE: a transaction has ONE AND ONLY ONE .SaveChanges()
                    context.SaveChanges();
                }
            }
        }//eom
        public void MoveTrack(MoveTrackItem moveTrack)
        {
            using (var context = new ChinookSystemContext())
            {
                //code to go here 



                if (string.IsNullOrEmpty(moveTrack.PlaylistName))
                {
                    //there is a data error
                    //setting up an error message
                    //brokenRules.Add(new BusinessRuleException<datatype>("my message", //nameof("fieldnameinerror"), fieldnameinerror));
                    brokenRules.Add(new BusinessRuleException<string>("playlist name is missing, Unable to add track", "Playlist Name", moveTrack.PlaylistName));
                }
                if (string.IsNullOrEmpty(moveTrack.UserName))
                {
                    brokenRules.Add(new BusinessRuleException<string>("username is missing, Unable to add a track", "User Name", moveTrack.UserName));
                }
                
                if(moveTrack.TrackId  <=0)
                {
                    brokenRules.Add(new BusinessRuleException<int>("Invalid track identifier, Unable to move a track", "Track Identifier", moveTrack.TrackId));
                }

                if (moveTrack.TrackNumber <= 0)
                {
                    brokenRules.Add(new BusinessRuleException<int>("Invalid track number, Unable to move a track(s)", "Track Number", moveTrack.TrackNumber));
                }

                Playlist exist = context.Playlists.Where(x => x.Name.Equals(moveTrack.PlaylistName) && x.UserName.Equals(moveTrack.UserName)).FirstOrDefault();

                if (exist == null)
                {
                    brokenRules.Add(new BusinessRuleException<string>("playlist does not exist", nameof(MoveTrackItem.PlaylistName), moveTrack.PlaylistName));
                }
                else
                {
                    //list of all tracks to be kept

                    //Chec
                    PlaylistTrack trackExist = context.PlaylistTracks.Where(x => x.Playlist.Name.Equals(moveTrack.PlaylistName) && x.TrackId == moveTrack.TrackId && x.Playlist.UserName.Equals(moveTrack.UserName) && x.TrackNumber == moveTrack.TrackNumber).FirstOrDefault();




                    if (trackExist == null)
                    {
                        brokenRules.Add(new BusinessRuleException<string>("track name does not exist", "Track Name", moveTrack.PlaylistName));
                    }
                    else
                    {
                        if (moveTrack.Direction.Equals("up"))
                        {
                            if (trackExist.TrackNumber == 1)
                            {
                                brokenRules.Add(new BusinessRuleException<string>("playlist trac k already on the top.Refresh your display", nameof(Track.Name), trackExist.Track.Name));
                            }
                            else
                            {
                                //do tthe moeve

                                //get the adajacent track

                                PlaylistTrack otherTrack = context.PlaylistTracks.Where(x => x.Playlist.Name.Equals(moveTrack.PlaylistName) && x.Playlist.UserName.Equals(moveTrack.UserName) && x.TrackNumber == (trackExist.TrackNumber - 1)).Select(x => x).FirstOrDefault();
                                if (otherTrack == null)
                                {
                                    brokenRules.Add(new BusinessRuleException<string>("playlist to swap seems to be missing.Refresh your display", nameof(MoveTrackItem.PlaylistName), moveTrack.PlaylistName));
                                }
                                else
                                {
                                    //good to swap
                                    //the swap is a mattter of changing track number value

                                    trackExist.TrackNumber--;
                                    otherTrack.TrackNumber++;

                                    //staging

                                    context.Entry(trackExist).Property(nameof(PlaylistTrack.TrackNumber)).IsModified = true;

                                    context.Entry(otherTrack).Property(nameof(PlaylistTrack.TrackNumber)).IsModified = true;
                                }
                            }
                        }
                        else
                        {
                            //down
                            if (moveTrack.Direction.Equals("down"))
                            {


                                var play = context.PlaylistTracks.Where(x => x.Playlist.Name.Equals(moveTrack.PlaylistName) && x.Playlist.UserName.Equals(moveTrack.UserName)).Count(); 
                                if (trackExist.TrackNumber == play )
                                {
                                    brokenRules.Add(new BusinessRuleException<string>("playlist track already on the bottom.Refresh your display", nameof(Track.Name), trackExist.Track.Name));
                                }
                                else
                                {
                                    //do tthe moeve

                                    //get the adajacent track

                                    PlaylistTrack otherTrack = context.PlaylistTracks.Where(x => x.Playlist.Name.Equals(moveTrack.PlaylistName) && x.Playlist.UserName.Equals(moveTrack.UserName) && x.TrackNumber == (trackExist.TrackNumber + 1)).Select(x => x).FirstOrDefault();
                                    if (otherTrack == null)
                                    {
                                        brokenRules.Add(new BusinessRuleException<string>("playlist to swap seems to be missing.Refresh your display", nameof(MoveTrackItem.PlaylistName), moveTrack.PlaylistName));
                                    }
                                    else
                                    {
                                        //good to swap
                                        //the swap is a mattter of changing track number value

                                        trackExist.TrackNumber++;
                                        otherTrack.TrackNumber--;

                                        //staging

                                        context.Entry(trackExist).Property(nameof(PlaylistTrack.TrackNumber)).IsModified = true;

                                        context.Entry(otherTrack).Property(nameof(PlaylistTrack.TrackNumber)).IsModified = true;
                                    }
                                }

                            }
                        }
                    }    //remove the desired tracks

                    if (brokenRules.Count > 0)
                    {
                        throw new BusinessRuleCollectionException("Track Move concerns: ", brokenRules);
                    }
                    else
                    {
                        context.SaveChanges();
                    }


                }  

            }
        }//eom


        public void DeleteTracks(string username, string playlistname, List<int> trackstodelete)
        {
            Playlist playlistExist = null;
            //PlaylistTrack playlisttrackExist = null;
            int trackNumber = 0;
            using (var context = new ChinookSystemContext())
            {
                //code to go here

                if (string.IsNullOrEmpty(playlistname))
                {
                    //there is a data error
                    //setting up an error message
                    //brokenRules.Add(new BusinessRuleException<datatype>("my message", //nameof("fieldnameinerror"), fieldnameinerror));
                    brokenRules.Add(new BusinessRuleException<string>("playlist name is missing, Unable to add track", "Playlist Name", playlistname));
                }
                if (string.IsNullOrEmpty(username))
                {
                    brokenRules.Add(new BusinessRuleException<string>("username is missing, Unable to add a track", "User Name", username));
                }
                if (trackstodelete.Count == 0)
                {
                    brokenRules.Add(new BusinessRuleException<int>("No tracks we selected. Unable to remove track(s)", "Track list count", 0));
                }

                playlistExist = context.Playlists.Where(x => x.Name.Equals(playlistname) && x.UserName.Equals(username)).FirstOrDefault();

                if (playlistExist == null)
                {
                    brokenRules.Add(new BusinessRuleException<string>("playlist does not exist", nameof(playlistname), playlistname));
                }
                else
                {
                    //list of all tracks to be kept

                    var trackskept = context.PlaylistTracks.Where(x => x.Playlist.UserName.Equals(username) && x.Playlist.Name.Equals(trackNumber) && !trackstodelete.Any(tod => tod == x.TrackId)).OrderBy(x => x.TrackNumber).Select(x => x).ToList();

                    //remove the desired tracks

                    PlaylistTrack item = null;
                    foreach (var deleteRecord in trackstodelete)
                    {
                        item = context.PlaylistTracks.Where(x => x.Playlist.UserName.Equals(username) && x.Playlist.Name.Equals(trackNumber) && x.TrackId == deleteRecord).FirstOrDefault();


                        //delete
                        //stage (parent.navproperty.remove)
                        if (item != null)
                        {
                            playlistExist.PlaylistTracks.Remove(item);
                        }

                    }



                    //re-sequence the list
                    //option a) use a list and update the records of the list
                    //option b) to delete all children records and re add onl the necessary kept records

                    //Within this example, you'll see an update specific columns of a records(option a)

                    trackNumber = 1;

                    foreach (var track in trackskept)
                    {
                        track.TrackNumber = trackNumber;
                        //stage a single field to be updated 
                        context.Entry(track).Property(nameof(PlaylistTrack.TrackNumber)).IsModified = true;
                        trackNumber++;
                    }
                }
                //commit?
                if (brokenRules.Count > 0)
                {
                    throw new BusinessRuleCollectionException("Track Removal concerns: ", brokenRules);

                }
                else
                {
                    context.SaveChanges();
                }
            }
        }//eom
    }
}
