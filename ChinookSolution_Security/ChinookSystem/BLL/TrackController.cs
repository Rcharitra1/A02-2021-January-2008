﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using ChinookSystem.Entities;
using ChinookSystem.ViewModels;
using ChinookSystem.DAL;
using System.ComponentModel;
#endregion

namespace ChinookSystem.BLL
{
    [DataObject]
    public class TrackController
    {
        //[DataObjectMethod(DataObjectMethodType.Select, false)]
        //public List<Track> Track_List()
        //{
        //    using (var context = new ChinookSystemContext())
        //    {
        //        return context.Tracks.ToList();
        //    }
        //}

        //[DataObjectMethod(DataObjectMethodType.Select, false)]
        //public Track Track_Find(int trackid)
        //{
        //    using (var context = new ChinookContext())
        //    {
        //        return context.Tracks.Find(trackid);
        //    }
        //}

        //[DataObjectMethod(DataObjectMethodType.Select, false)]
        //public List<Track> Track_GetByAlbumId(int albumid)
        //{
        //    using (var context = new ChinookContext())
        //    {
        //        var results = from aRowOn in context.Tracks
        //                      where aRowOn.AlbumId.HasValue
        //                      && aRowOn.AlbumId == albumid
        //                      select aRowOn;
        //        return results.ToList();
        //    }
        //}

        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<TrackList> List_TracksForPlaylistSelection(string tracksby, string arg)
        {
            using (var context = new ChinookSystemContext())
            {
                List<TrackList> results = null;
                //if (tracksby.Equals("Artist"))
                //{

                //    results = context.Tracks.Where(x => x.Album.Artist.Name.Contains(arg))
                //      .Select(x => new TrackList
                //      {
                //          TrackID = x.TrackId,
                //          Name = x.Name,
                //          Title = x.Album.Title,
                //          ArtistName = x.Album.Artist.Name,
                //          GenreName = x.Genre.Name,
                //          Composer = x.Composer,
                //          Milliseconds = x.Milliseconds,
                //          Bytes = x.Bytes,
                //          UnitPrice = x.UnitPrice
                //      }).ToList();
                //}else
                //{
                //    results = context.Tracks.Where(x => x.Album.Title.Contains(arg))
                //      .Select(x => new TrackList
                //      {
                //          TrackID = x.TrackId,
                //          Name = x.Name,
                //          Title = x.Album.Title,
                //          ArtistName = x.Album.Artist.Name,
                //          GenreName = x.Genre.Name,
                //          Composer = x.Composer,
                //          Milliseconds = x.Milliseconds,
                //          Bytes = x.Bytes,
                //          UnitPrice = x.UnitPrice
                //      }).ToList();
                //}



                //results = context.Tracks.Where(x => tracksby.Equals("Artist") ? x.Album.Artist.Name.Contains(arg) : (tracksby.Equals("Genre") ? (x.GenreId.ToString() == arg) : x.Album.Title.Contains(arg)))
                //  .Select(x => new TrackList
                //  {
                //      TrackID = x.TrackId,
                //      Name = x.Name,
                //      Title = x.Album.Title,
                //      ArtistName = x.Album.Artist.Name,
                //      GenreName = x.Genre.Name,
                //      Composer = x.Composer,
                //      Milliseconds = x.Milliseconds,
                //      Bytes = x.Bytes,
                //      UnitPrice = x.UnitPrice
                //  }).ToList();

                //return results;



                results = context.Tracks.Where(x => tracksby.Equals("Artist") ? x.Album.Artist.Name.Contains(arg) : (tracksby.Equals("Genre") ? (x.Genre.Name.Equals(arg)) : x.Album.Title.Contains(arg)))
                  .Select(x => new TrackList
                  {
                      TrackID = x.TrackId,
                      Name = x.Name,
                      Title = x.Album.Title,
                      ArtistName = x.Album.Artist.Name,
                      GenreName = x.Genre.Name,
                      Composer = x.Composer,
                      Milliseconds = x.Milliseconds,
                      Bytes = x.Bytes,
                      UnitPrice = x.UnitPrice
                  }).ToList();

                return results;
            }
        }//eom

       
    }//eoc
}