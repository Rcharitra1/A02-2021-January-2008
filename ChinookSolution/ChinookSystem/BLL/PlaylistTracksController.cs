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

                var results = context.PlaylistTracks.Where(x => x.Playlist.Name.Equals(playlistname) && x.Playlist.UserName.Equals(username)).OrderBy(x=> x.TrackNumber).Select(x=> new UserPlaylistTrack
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

        public void Add_TrackToPLaylist(string playlistname, string username, int trackid, string song)
        {
            Playlist playlistExist=null;
            PlaylistTrack playlisttrackExist=null;
            int trackNumber=0;
            using (var context = new ChinookSystemContext())
            {
                //this class is in what is called Business logic layer
                //Business logic is the rules of your business
                //Business logic ensures that rules and data are what is expected 
                //Rules:
                //      rule: a track can only exist once on a playlist
                //      rule: playlist names can only be used once for a user, different users may have the same playlist name
                //      rule: each track on a playlist is assigned a continious track number
                //The BLL methos should alos ensure that data exists for the processing of the transaction

                if(string.IsNullOrEmpty(playlistname))
                {
                    //there is a data error
                    //setting up an error message
                    //brokenRules.Add(new BusinessRuleException<datatype>("my message", //nameof("fieldnameinerror"), fieldnameinerror));
                    brokenRules.Add(new BusinessRuleException<string>("playlist name is missing, Unable to add track", "Playlist Name", playlistname));
                }
                if(string.IsNullOrEmpty(username))
                {
                    brokenRules.Add(new BusinessRuleException<string>("username is missing, Unable to add a track", "User Name", username));
                }
                if (brokenRules.Count == 0)
                {
                    //does the playlist already exist
                    playlistExist = context.Playlists.Where(x => x.Name.Equals(playlistname) && x.UserName.Equals(username)).FirstOrDefault();
                    if(playlistExist == null)
                    {
                        //new playlist
                        //task:
                        //  Create a new instance of the playlist class
                        //  Load the instance with data
                        //  Set the track number to 1

                        playlistExist = new Playlist() 
                                {
                                    Name = playlistname,
                                    UserName= username  
                                };
                        context.Playlists.Add(playlistExist);
                        trackNumber = 1;
                    }
                    else
                    {
                        //existing playlist
                        //task:
                        //  Does the track already exists on the playlist? If so, error
                        //  If not, find the highest current trackNumber, increment by 1

                        
                    }
                }
                else
                {
                    
                }

                

             
            }
        }//eom
        public void MoveTrack(string username, string playlistname, int trackid, int tracknumber, string direction)
        {
            using (var context = new ChinookSystemContext())
            {
                //code to go here 

            }
        }//eom


        public void DeleteTracks(string username, string playlistname, List<int> trackstodelete)
        {
            using (var context = new ChinookSystemContext())
            {
               //code to go here


            }
        }//eom
    }
}
