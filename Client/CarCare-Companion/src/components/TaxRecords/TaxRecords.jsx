import { useEffect, useState } from 'react';

import { Link } from 'react-router-dom';

import { NotificationHandler } from '../../utils/NotificationHandler';

import useAxiosPrivate from '../../hooks/useAxiosPrivate';

import IsLoadingHOC from '../Common/IsLoadingHoc';

import TaxRecordCard from './TaxRecordCard';
import TaxRecordsStatistics from './TaxRecordsStatistics/TaxRecordsStatistics';

import './TaxRecords.css'


const TaxRecords = (props) => {

    const { setLoading } = props;

    const axiosPrivate = useAxiosPrivate();

    const [taxRecords, setTaxRecords] = useState([]);


    useEffect(() => {
        let isMounted = true;
        const controller = new AbortController();

        const getRecords = async () => {
            try {
                const response = await axiosPrivate.get('/TaxRecords', {
                    signal: controller.signal
                });
                isMounted && setTaxRecords(taxRecords => response.data);
            } catch (err) {
                NotificationHandler(err);
                navigate('/login', { state: { from: location }, replace: true });
            }
            finally{
                setLoading(false);
            }
        }

        getRecords();

        return () => {
            isMounted = false;
            isMounted && controller.abort();
        }
    }, [])


    return (
        <section className='tax-records-section'>
            <div className='tax-records-container'>
                <div className="taxes-statistics">
                    <Link id = "add-tax-record" to="/TaxRecords/Add">Add tax record</Link>
                    <div className='tax-records-statistics'><TaxRecordsStatistics/></div>
                </div>
                <div className="tax-records-list">
                    {
                        taxRecords.length > 0
                            ?
                            taxRecords.map(tr => <TaxRecordCard key={tr.id} taxRecordDetails={tr} />)
                            :
                            <div>You haven't added any tax records yet.</div>
                    }
                </div>
            </div>
        </section>
    )
}


export default IsLoadingHOC(TaxRecords);