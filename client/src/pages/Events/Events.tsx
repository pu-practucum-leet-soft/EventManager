import classNames from "classnames";

import EventsList from "@components/EventsList";
import Section from "@components/UI/Section";

import styles from "./Events.module.scss";
import Button from "@components/UI/Button";
import Icon from "@components/UI/Icon";
import eventQueries, { eventCacheTags } from "@queries/api/eventQueries";
import { useQuery } from "@tanstack/react-query";
import { eventViewModelsToCardProps } from "@utils/adapters/eventAdapter";

const EventsPage = () => {
  const { data, isLoading, error } = useQuery({
    queryKey: [eventCacheTags.index],
    queryFn: async () => {
      const res = await eventQueries.getAll();

      return res.data;
    },
    retry: false,
  });

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (error || !data) {
    return <div>Error loading data</div>;
  }

  return (
    <div className={styles.Events}>
      <h1>Events</h1>
      <div className={styles.EventsContainer}>
        <div className={styles.FilterBar}>
          <div className={styles.FilterFieldGroup}>
            <label htmlFor="search">Event Name</label>
            <input
              id="search"
              name="search"
              className={classNames(styles.FilterField, styles.SearchField)}
              type="text"
              placeholder="Search events..."
            />
          </div>
          <div className={styles.FilterFieldGroup}>
            <label htmlFor="location">Location</label>
            <select
              className={styles.FilterField}
              name="location"
              id="location"
            >
              <option value="">Select Location</option>
              <option value="location1">Location 1</option>
              <option value="location2">Location 2</option>
            </select>
          </div>
          <div className={styles.FilterFieldGroup}>
            <label htmlFor="startDate">Start Date</label>
            <input
              id="startDate"
              name="startDate"
              className={styles.FilterField}
              type="date"
              placeholder="Select Start Date"
            />
          </div>
          <div className={styles.FilterFieldGroup}>
            <label htmlFor="endDate">End Date</label>
            <input
              id="endDate"
              name="endDate"
              className={styles.FilterField}
              type="date"
              placeholder="Select End Date"
            />
          </div>
          <Button
            inline
            variant="primary"
            color="primary"
            border="rounded"
            className={styles.FilterButton}
          >
            Filter
          </Button>
        </div>
        <Section className={styles.EventList}>
          <EventsList
            events={eventViewModelsToCardProps(data.events)}
            noEventsMessage="No events found."
          />
        </Section>
        <div className={styles.Actions}>
          <div className={styles.Pagination}>
            <Button
              variant="text"
              color="primary"
              border="rounded"
              className={styles.ButtonLeft}
            >
              <Icon className={styles.PageIconLeft} name="arrow-left" />
            </Button>
            <span className={styles.PageInfo}>1</span>
            <Button
              variant="text"
              color="primary"
              border="block"
              className={styles.ButtonRight}
            >
              <Icon className={styles.PageIconRight} name="arrow-right" />
            </Button>
          </div>
          <div className={styles.AddEvent}>
            <Button variant="primary" color="primary" border="rounded">
              Add Event
            </Button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default EventsPage;
