using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Additonal Namespaces
using ChinookSystem.BLL;
using ChinookSystem.ViewModels;

#endregion

namespace WebApp.SamplePages
{
    public partial class ManagePlaylist : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TracksSelectionList.DataSource = null;
        }

        

        protected void ArtistFetch_Click(object sender, EventArgs e)
        {


            TracksBy.Text = "Artist";
            //The Hidden field content access is .Value Not .Text
            if(string.IsNullOrEmpty(ArtistName.Text))
            {
                MessageUserControl.ShowInfo("You did not supply an artist name");
                SearchArg.Value = "xdcxe";
            }
            else
            {

                SearchArg.Value = ArtistName.Text;

            }
            //to force the re-execution of an ODS attached to a display control
            //rebind the display control
            TracksSelectionList.DataBind();


        }


        protected void GenreFetch_Click(object sender, EventArgs e)
        {

            //code to go here

            TracksBy.Text = "Genre";

            //if(GenreDDL.SelectedIndex<=-1)
            //{
            //    MessageUserControl.ShowInfo("Pls select a genre to begin");
            //}else
            //{
            //    SearchArg.Value = GenreDDL.SelectedValue;
            //}

            SearchArg.Value = GenreDDL.SelectedItem.Text;            
            TracksSelectionList.DataBind();

        }

        protected void AlbumFetch_Click(object sender, EventArgs e)
        {

            TracksBy.Text = "Album";
            //The Hidden field content access is .Value Not .Text
            if (string.IsNullOrEmpty(AlbumTitle.Text))
            {
                MessageUserControl.ShowInfo("You did not supply an album name");
                SearchArg.Value = "xdcxe";
            }
            else
            {

                SearchArg.Value = AlbumTitle.Text;

            }
            //to force the re-execution of an ODS attached to a display control
            //rebind the display control
            TracksSelectionList.DataBind();


        }

        protected void PlayListFetch_Click(object sender, EventArgs e)
        {
            //username is coming from the system via security
            //since security has yet to be installed, a defualt will be setup for the username value

            string username = "HansenB";
            if(string.IsNullOrEmpty(PlaylistName.Text))
            {
                MessageUserControl.ShowInfo("Playlist Search", "No playlist name was supplied");

            }
            else
            {
                MessageUserControl.TryRun(() =>
                {
                    PlaylistTracksController sysmgr = new PlaylistTracksController();
                    RefreshPlayList(sysmgr, username);
                }, "Playlist search", "view the requested playlist below");
                
            }
 
        }

        protected void RefreshPlayList(PlaylistTracksController sysmgr, string username)
        {
            List<UserPlaylistTrack> info = sysmgr.List_TracksForPlaylist(PlaylistName.Text, username);
            PlayList.DataSource = info;
            PlayList.DataBind();
        }

        protected void MoveDown_Click(object sender, EventArgs e)
        {
            //code to go here



            if (string.IsNullOrEmpty(PlaylistName.Text))
            {
                MessageUserControl.ShowInfo("Missing Data", "Enter a playlist name");
            }
            else
            {
                MoveTrackItem moveTrack = new MoveTrackItem();
                if (PlayList.Rows.Count == 0)
                {
                    MessageUserControl.ShowInfo("Track movement", "You must a have a playlist for movement");
                }
                else
                {
                    ;

                    int rowsSelected = 0;

                    CheckBox trackSelection = null;

                    //treverse the grid view control PlayList
                    //you could do the same code using foreach()




                    foreach (GridViewRow row in PlayList.Rows)
                    {
                        trackSelection = row.FindControl("Selected") as CheckBox;
                        if (trackSelection.Checked)
                        {
                            rowsSelected++;
                            moveTrack.TrackId = int.Parse((trackSelection.FindControl("TrackID") as Label).Text);

                            moveTrack.TrackNumber = int.Parse((trackSelection.FindControl("TrackNumber") as Label).Text);


                        }
                    }

                    //was a single song selected

                    switch (rowsSelected)
                    {
                        case 0:
                            {
                                MessageUserControl.ShowInfo("Track movement Concern", "You must select one song to remove");
                                break;

                            }

                        case 1:
                            {
                                if(moveTrack.TrackNumber ==PlayList.Rows.Count)
                                {
                                    MessageUserControl.ShowInfo("Track Movement concern", "Track selected already at the bottom of your playlist");
                                }else
                                {
                                    moveTrack.Direction = "down";

                                    MoveTrack(moveTrack);
                                }
                                

                                break;
                            }

                        default:
                            {
                                MessageUserControl.ShowInfo("Track movement Concern", "You must  select at only one song to remove");
                                break;
                            }
                    }


                }
            }


        }

        protected void MoveUp_Click(object sender, EventArgs e)
        {
            //code to go here

            if (string.IsNullOrEmpty(PlaylistName.Text))
            {
                MessageUserControl.ShowInfo("Missing Data", "Enter a playlist name");
            }
            else
            {
                MoveTrackItem moveTrack = new MoveTrackItem();
                if (PlayList.Rows.Count == 0)
                {
                    MessageUserControl.ShowInfo("Track movement", "You must a have a playlist for movement");
                }
                else
                {
                    ;

                    int rowsSelected = 0;

                    CheckBox trackSelection = null;

                    //treverse the grid view control PlayList
                    //you could do the same code using foreach()




                    foreach (GridViewRow row in PlayList.Rows)
                    {
                        trackSelection = row.FindControl("Selected") as CheckBox;
                        if (trackSelection.Checked)
                        {
                            rowsSelected++;
                            moveTrack.TrackId = int.Parse((trackSelection.FindControl("TrackID") as Label).Text);

                            moveTrack.TrackNumber = int.Parse((trackSelection.FindControl("TrackNumber") as Label).Text);


                        }
                    }

                    //was a single song selected

                    switch (rowsSelected)
                    {
                        case 0:
                            {
                                MessageUserControl.ShowInfo("Track movement Concern", "You must select one song to remove");
                                break;

                            }

                        case 1:
                            {
                                if (moveTrack.TrackNumber == 1)
                                {
                                    MessageUserControl.ShowInfo("Track Movement concern", "Track selected already at the top of your playlist");
                                }
                                else
                                {
                                    moveTrack.Direction = "up";

                                    MoveTrack(moveTrack);
                                }


                                break;
                            }

                        default:
                            {
                                MessageUserControl.ShowInfo("Track movement Concern", "You must  select at only one song to remove");
                                break;
                            }
                    }


                }
            }

        }

        protected void MoveTrack(MoveTrackItem moveTrack)
        {
            //call BLL to move track
            string username = "HansenB";
            moveTrack.UserName = username;
            moveTrack.PlaylistName = PlaylistName.Text;

            MessageUserControl.TryRun(() =>
            {
                PlaylistTracksController sysmgr = new PlaylistTracksController();

                sysmgr.MoveTrack(moveTrack);
                RefreshPlayList(sysmgr, username);
            }, "Track Movement", "Track moved successfully");

        }


        protected void DeleteTrack_Click(object sender, EventArgs e)
        {
            //code to go here

            string username = "HansenB";

            if (string.IsNullOrEmpty(PlaylistName.Text))
            {
                MessageUserControl.ShowInfo("Missing Data", "Enter a playlist name");
            }
            else
            {
                if (PlayList.Rows.Count == 0)
                {
                    MessageUserControl.ShowInfo("Track Removal", "You must a have a playlist");
                }
                else
                {
                    List<int> trackids = new List<int>();

                    int rowsSelected = 0;

                    CheckBox trackSelection = null;

                    //treverse the grid view control PlayList
                    //you could do the same code using foreach()




                    foreach (GridViewRow row in PlayList.Rows)
                    {
                        trackSelection = row.FindControl("Selected") as CheckBox;
                        if (trackSelection.Checked)
                        {
                            rowsSelected++;
                            trackids.Add(int.Parse((row.FindControl("TrackId") as Label).Text));
                        }
                    }


                    if (rowsSelected == 0)
                    {
                        MessageUserControl.ShowInfo("Delete Concern", "You must select at least on song to remove");
                    }
                    else
                    {
                        MessageUserControl.TryRun(() =>
                        {
                            var sysmgr = new PlaylistTracksController();

                            sysmgr.DeleteTracks(username, PlaylistName.Text, trackids);
                            RefreshPlayList(sysmgr, username);
                        }, "Track Removal", "Record(s) successfully deleted");
                    }

                }
            }

        }

        protected void TracksSelectionList_ItemCommand(object sender, 
            ListViewCommandEventArgs e)
        {
            string username = "HansenB";
            //until security is implemented
            //form event validation: presence
            if(string.IsNullOrEmpty(PlaylistName.Text))
            {
                MessageUserControl.ShowInfo("Missing data", "Enter a playlist name");
            }
            else
            {
                MessageUserControl.TryRun(() =>
                {
                //logic for your coding block
                PlaylistTracksController sysmgr = new PlaylistTracksController();
                    //access a specific field on the selected ListView row
                string song = (e.Item.FindControl("NameLabel") as Label).Text;
                    sysmgr.Add_TrackToPLaylist(PlaylistName.Text, username,
                        int.Parse(e.CommandArgument.ToString()),song
                        );
                    RefreshPlayList(sysmgr, username);
                   
                }, "Add Track to Playlist", "Track successfully added to the playlist");
            }
            
        }

        #region MessageUserControl Error Handling for ODS
        protected void SelectCheckForException(object sender,
             ObjectDataSourceStatusEventArgs e)
        {
            MessageUserControl.HandleDataBoundException(e);
        }
        protected void InsertCheckForException(object sender,
             ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                MessageUserControl.ShowInfo("Process success", "Album has been added");
            }
            else
            {
                MessageUserControl.HandleDataBoundException(e);
            }

        }
        protected void UpdateCheckForException(object sender,
             ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                MessageUserControl.ShowInfo("Process success", "Album has been updated");
            }
            else
            {
                MessageUserControl.HandleDataBoundException(e);
            }

        }
        protected void DeleteCheckForException(object sender,
             ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                MessageUserControl.ShowInfo("Process success", "Album has been removed");
            }
            else
            {
                MessageUserControl.HandleDataBoundException(e);
            }

        }
        #endregion

    }
}