import classNames from "classnames";

import EventsList from "@components/EventsList";
import Section from "@components/UI/Section";

import styles from "./Events.module.scss";
import Button from "@components/UI/Button";
import Icon from "@components/UI/Icon";
import eventQueries, { eventCacheTags } from "@queries/api/eventQueries";
import { useMutation, useQuery } from "@tanstack/react-query";
import { eventViewModelsToCardProps } from "@utils/adapters/eventAdapter";
import { useDispatch } from "react-redux";
import { openAddEventModal } from "@redux/slices/modalSlice";
import { useState } from "react";
import { EventViewModel } from "@queries/types/event.types";

type FilterData = {
  title: string;
  location: string;
  startDate: string;
};

const EventsPage = () => {
  const dispatch = useDispatch();
  const [filterData, scssetFilterData] = useState<FilterData>({
    title: "",
    location: "",
    startDate: "",
  });
  const [events, setEvents] = useState<Array<EventViewModel>>([]);

  const { data, isLoading, error } = useQuery({
    queryKey: [eventCacheTags.index],
    queryFn: async () => {
      const res = await eventQueries.getAll();

      setEvents(res.data.events);
      return res.data;
    },
    retry: false,
  });

  const filterMutate = useMutation({
    mutationFn: async (filterQuery: string) => {
      const res = await eventQueries.filterEvents(filterQuery);

      return res.data;
    },
    onSuccess: (data) => {
      setEvents(data.events);
    },
  });

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (error || !data) {
    return <div>Error loading data</div>;
  }

  const handleOnAddEvent = () => {
    dispatch(openAddEventModal({}));
  };

  const handleFilter = () => {
    const { title, location, startDate } = filterData;
    let query = "";

    if (title.trim() != "")
      query = query.length > 0 ? `${query}&Title=${title}` : `?Title=${title}`;

    if (location.trim() != "")
      query =
        query.length > 0
          ? `${query}&Location=${location}`
          : `?Location=${location}`;

    if (startDate.trim() != "")
      query =
        query.length > 0
          ? `${query}&StartDate=${startDate}`
          : `?StartDate=${startDate}`;

    console.log(query);

    filterMutate.mutate(query);
  };

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
              value={filterData.title}
              onChange={(e) =>
                scssetFilterData({ ...filterData, title: e.target.value })
              }
            />
          </div>
          <div className={styles.FilterFieldGroup}>
            <label htmlFor="location">Location</label>
            <select
              className={styles.FilterField}
              name="location"
              id="location"
              value={filterData.location}
              onChange={(e) =>
                scssetFilterData({ ...filterData, location: e.target.value })
              }
            >
              <option value="">Select Location</option>
              <option value="Sofia, Bulgaria">Sofia, Bulgaria</option>
              <option value="Plovdiv, Bulgaria">Plovdiv, Bulgaria</option>
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
              value={filterData.startDate.toString()}
              onChange={(e) =>
                scssetFilterData({
                  ...filterData,
                  startDate: e.target.value,
                })
              }
            />
          </div>
          <Button
            inline
            variant="primary"
            color="primary"
            border="rounded"
            className={styles.FilterButton}
            onClick={handleFilter}
          >
            Filter
          </Button>
        </div>
        <Section className={styles.EventList}>
          <EventsList
            events={eventViewModelsToCardProps(events)}
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
            <Button
              variant="primary"
              color="primary"
              border="rounded"
              onClick={handleOnAddEvent}
            >
              Add Event
            </Button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default EventsPage;
