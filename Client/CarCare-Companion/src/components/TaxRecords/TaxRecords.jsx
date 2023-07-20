import { useEffect, useState } from 'react';

import { Link } from 'react-router-dom';

import { NotificationHandler } from '../../utils/NotificationHandler';

import { getAllTaxRecords } from '../../services/taxRecordsService';

import IsLoadingHOC from '../Common/IsLoadingHoc';

import TaxRecordCard from './TaxRecordCard';

import './TaxRecords.css'


const TaxRecords = (props) => {

    const { setLoading } = props;

    const [taxRecords, setTaxRecords] = useState([]);

    useEffect(() => {
        (async () => {
            try {
                let taxRecordsResult = await getAllTaxRecords();
                console.log(taxRecordsResult);
                setTaxRecords(taxRecords => taxRecordsResult);
                setLoading(false);
            }
            catch (error) {
                NotificationHandler(error);
                setLoading(false);
            }
        })()
    }, [])

    return (
        <section className='tax-records-section'>
            <div className='tax-records-container'>
                <div className="taxes-statistics">
                    <Link id = "add-tax-record" to="/TaxRecords/Add">Add tax record</Link>
                </div>
                <div className="tax-records-list">
                    {
                        taxRecords.length > 0
                            ?
                            taxRecords.map(sr => <TaxRecordCard key={trr.id} taxRecordDetails={tr} />)
                            :
                            <div>You haven't added any tax records yet.</div>
                    }
                </div>
            </div>
        </section>
    )
}


export default IsLoadingHOC(TaxRecords);