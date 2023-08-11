// Import the useAuthContext hook to access the authentication context.
import { useAuthContext } from "../../../contexts/AuthContext";

// Importing child components which are different statistics and records modules 
// that will be shown on the authenticated home page.

// Component to display service record statistics.
import ServiceRecordsStatistics from "./ServiceRecordsStatistics/ServiceRecordsStatistics";

// Component to display tax record statistics.
import TaxRecordsStatistics from "./TaxRecordsStatistics/TaxRecordsStatistics";

// Component to display trip statistics.
import TripStatiscis from "./TripsStatistics/TripsStatistics";

// Component to display the latest trips.
import LatestTrips from "./LatestTrips/LatestTrips";

// Component to display upcoming tax schedules.
import UpcomingTaxes from "./UpcomingTaxes/UpcomingTaxes";

// CSS styles specific to this component.
import './AuthenticatedHomePage.css';

const AuthenticatedHomePage = () => {

    // Use the useAuthContext hook to extract the user information.
    // This user object contains data about the authenticated user.
    const { user } = useAuthContext();

    return (
        <section className="home">
            <div className="container">
                <section className="authenticated-home-container">
                    <div className="user-overview-container">
                        <div className="user-welcoming">
                            <div>Hello, {user.email}.</div>
                            <div>Nice to see you again.</div>
                        </div>
                        <div className="all-user-statistics-container">
                            <h1>Overview of your records:</h1>
                            <ServiceRecordsStatistics />
                            <TaxRecordsStatistics />
                            <TripStatiscis />
                        </div>
                    </div>
                    <div className="user-recent-activites">
                        <UpcomingTaxes />
                        <LatestTrips />
                    </div>

                </section>
            </div>
        </section>
    );

}


export default AuthenticatedHomePage;