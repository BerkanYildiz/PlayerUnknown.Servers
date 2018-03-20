namespace PlayerUnknown.Logic.Matchmaking
{
    using System.Collections.Generic;

    public class Regions
    {
        /// <summary>
        /// Gets or sets the regions.
        /// </summary>
        private Dictionary<string, Region> Locations
        {
            get;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Regions"/> class.
        /// </summary>
        public Regions()
        {
            this.Locations = new Dictionary<string, Region>();
        }

        /// <summary>
        /// Adds the region.
        /// </summary>
        public void AddRegion(string Region)
        {
            if (this.Locations.ContainsKey(Region))
            {
                return;
            }

            this.Locations.Add(Region, new Region(Region));
        }

        /// <summary>
        /// Gets the region.
        /// </summary>
        /// <param name="Region">The region.</param>
        public Region GetRegion(string Region)
        {
            if (this.Locations.ContainsKey(Region))
            {
                return this.Locations[Region];
            }

            return null;
        }

        /// <summary>
        /// Tries to get the region.
        /// </summary>
        /// <param name="Region">The region.</param>
        /// <param name="Servers">The servers.</param>
        public bool TryGetRegion(string Region, out Region Servers)
        {
            if (this.Locations.TryGetValue(Region, out Servers))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes the region.
        /// </summary>
        /// <param name="Region">The region.</param>
        public void RemoveRegion(string Region)
        {
            if (this.Locations.ContainsKey(Region) == false)
            {
                return;
            }

            this.Locations.Remove(Region);
        }
    }
}
