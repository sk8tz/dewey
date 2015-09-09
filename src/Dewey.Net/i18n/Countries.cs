using System.Collections.Generic;

namespace Dewey.Net.i18n
{
    public static class Countries
    {
        public static Country UnitedStates
        {
            get
            {
                return new Country {
                    Name = "United States",
                    Localities = new List<Locality> {
                        new Locality {
                            Name = "Arizona"
                        },
                        new Locality {
                            Name = "Texas"
                        }
                    }
                };
            }
        }
    }
}
