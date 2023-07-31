import { useAuthContext } from "../../../contexts/AuthContext";

import IsLoadingHOC from "../../Common/IsLoadingHoc";

import ServiceRecordsStatistics from "./ServiceRecordsStatistics/ServiceRecordsStatistics";
import TaxRecordsStatistics from "./TaxRecordsStatistics/TaxRecordsStatistics";
import TripStatiscis from "./TripsStatistics/TripsStatistics";
import LatestTrips from "./LatestTrips/LatestTrips";
import UpcomingTaxes from "./UpcomingTaxes/UpcomingTaxes";

import './AuthenticatedHomePage.css';

const AuthenticatedHomePage = (props) => {

    const { user } = useAuthContext();

    const { setLoading } = props;

    setLoading(false);

    console.log(user);
    return (
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
                    <TripStatiscis/>
                </div>
            </div>
            <div className="user-recent-activites">
                <UpcomingTaxes/>
                <LatestTrips/>
            </div>

        </section>
    );

}


export default IsLoadingHOC(AuthenticatedHomePage);